using CL.Core.Domain;
using CL.Data.Context;
using CL.Data.Repository;
using CL.FakeData.EspecialidadeData;
using CL.FakeData.MedicoData;
using CL.Manager.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CL.Respository.Tests.Repository
{
    public class MedicoRepositoryTest : IDisposable
    {
        private readonly IMedicoRepository repository;
        private readonly ClContext context;
        private readonly Medico medico;
        private readonly MedicoFaker medicoFaker;

        public MedicoRepositoryTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ClContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            context = new ClContext(optionsBuilder.Options);
            repository = new MedicoRepository(context);

            medicoFaker = new MedicoFaker();
            medico = medicoFaker.Generate();
        }

        private async Task<List<Medico>> InsereMedicos()
        {
            List<Especialidade> especialidades = await InsereEspecialidades();

            var medicos = medicoFaker.Generate(100);
            foreach (var med in medicos)
            {
                med.Id = 0;
                var random = new Random();
                var listEsp = new List<Especialidade> {
                    especialidades.ElementAt(random.Next(especialidades.Count)),
                    especialidades.ElementAt(random.Next(especialidades.Count))
                };
                med.Especialidades = listEsp;
                await context.Medicos.AddAsync(med);
            }
            await context.SaveChangesAsync();
            return medicos;
        }

        private async Task<List<Especialidade>> InsereEspecialidades()
        {
            var especialidades = new EspecialidadeFaker().Generate(100);

            foreach (var especialidade in especialidades)
            {
                especialidade.Id = 0;
                await context.Especialidades.AddAsync(especialidade);
            }
            await context.SaveChangesAsync();
            return especialidades;
        }

        [Fact]
        public async Task GetMedicosAsync_ComRetorno()
        {
            var registros = await InsereMedicos();
            var retorno = await repository.GetMedicosAsync();

            retorno.Should().HaveCount(registros.Count);
            retorno.First().Especialidades.Should().NotBeNull();
        }

        [Fact]
        public async Task GetMedicosAsync_Vazio()
        {
            var retorno = await repository.GetMedicosAsync();
            retorno.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetMedicoAsync_Encontrado()
        {
            var registros = await InsereMedicos();
            var retorno = await repository.GetMedicoAsync(registros.First().Id);
            retorno.Nome.Should().Be(registros.First().Nome);
            retorno.CRM.Should().Be(registros.First().CRM);
            retorno.Especialidades.Should().HaveCount(registros.First().Especialidades.Count);
        }

        [Fact]
        public async Task GetMedicoAsync_NaoEncontrado()
        {
            var retorno = await repository.GetMedicoAsync(1);
            retorno.Should().BeNull();
        }

        [Fact]
        public async Task InsertMedicoAsync_Sucesso()
        {
            var especialidades = await InsereEspecialidades();
            var random = new Random();
            var listEsp = new List<Especialidade> {
                    especialidades.ElementAt(random.Next(especialidades.Count)),
                    especialidades.ElementAt(random.Next(especialidades.Count))
                };
            medico.Especialidades = listEsp;
            var retorno = await repository.InsertMedicoAsync(medico);
            retorno.Should().BeEquivalentTo(medico);
        }

        [Fact]
        public async Task UpdateMedicoAsync_Sucesso()
        {
            var registros = await InsereMedicos();
            var medicoAlterado = medicoFaker.Generate();
            medicoAlterado.Id = registros.First().Id;
            var retorno = await repository.UpdateMedicoAsync(medicoAlterado);
            retorno.Should().BeEquivalentTo(medicoAlterado);
        }

        [Fact]
        public async Task UpdateMedicoAsync_AdicionaEspecialidade()
        {
            await InsereMedicos();

            var medicoAlterado = await context.Medicos.Include(p => p.Especialidades).AsNoTracking().FirstAsync();
            var especialidade = await context.Especialidades
                                                .Where(p => !medicoAlterado
                                                            .Especialidades
                                                            .Select(i => i.Id)
                                                            .Contains(p.Id))
                                                .AsNoTracking()
                                                .FirstAsync();
            medicoAlterado.Especialidades.Add(especialidade);
            var retorno = await repository.UpdateMedicoAsync(medicoAlterado);

            retorno.Especialidades.Should().HaveCount(medicoAlterado.Especialidades.Count);
        }

        [Fact]
        public async Task UpdateMedicoAsync_RemoveEspecialidade()
        {
            await InsereMedicos();
            var medicoAlterado = await context.Medicos.Include(p => p.Especialidades).AsNoTracking().FirstAsync();
            medicoAlterado.Especialidades.Remove(medicoAlterado.Especialidades.First());
            var retorno = await repository.UpdateMedicoAsync(medicoAlterado);
            retorno.Especialidades.Should().HaveCount(medicoAlterado.Especialidades.Count);
            retorno.Especialidades.First().Id.Should().Be(medicoAlterado.Especialidades.First().Id);
        }

        [Fact]
        public async Task UpdateMedicoAsync_RemoveTodasEspecialidades()
        {
            var registros = await InsereMedicos();
            var medicoAlterado = registros.First();
            medicoAlterado.Especialidades.Clear();
            var retorno = await repository.UpdateMedicoAsync(medicoAlterado);
            retorno.Should().BeEquivalentTo(medicoAlterado);
        }

        [Fact]
        public async Task UpdateMedicoAsync_NaoEncontrado()
        {
            var retorno = await repository.UpdateMedicoAsync(medico);
            retorno.Should().BeNull();
        }

        [Fact]
        public async Task DeleteMedicoAsync_Sucesso()
        {
            var registros = await InsereMedicos();
            var retorno = await repository.DeleteMedicoAsync(registros.First().Id);
            retorno.Should().BeEquivalentTo(registros.First());
        }

        [Fact]
        public async Task DeleteMedicoAsync_NaoEncontrado()
        {
            var retorno = await repository.DeleteMedicoAsync(1);
            retorno.Should().BeNull();
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
        }
    }
}