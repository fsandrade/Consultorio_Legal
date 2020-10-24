using CL.Core.Domain;
using CL.Data.Context;
using CL.Manager.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CL.Data.Repository
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly ClContext context;

        public MedicoRepository(ClContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Medico>> GetMedicosAsync()
        {
            return await context.Medicos
              .Include(p => p.Especialidades)
              .AsNoTracking().ToListAsync();
        }

        public async Task<Medico> GetMedicoAsync(int id)
        {
            return await context.Medicos
                .Include(p => p.Especialidades)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Medico> InsertMedicoAsync(Medico medico)
        {
            await context.Medicos.AddAsync(medico);
            await InsertMedicoEspecilidades(medico);
            await context.SaveChangesAsync();
            return medico;
        }

        private async Task InsertMedicoEspecilidades(Medico medico)
        {
            foreach (var especialidade in medico.Especialidades)
            {
                var especialidadeConsultada = await context.Especialidades.AsNoTracking().FirstAsync(p => p.Id == especialidade.Id);
                context.Entry(especialidade).CurrentValues.SetValues(especialidadeConsultada);
            }
        }

        public async Task<Medico> UpdateMedicoAsync(Medico medico)
        {
            var medicoConsultado = await context.Medicos
                                        .Include(p => p.Especialidades)
                                        .SingleOrDefaultAsync(p => p.Id == medico.Id);
            if (medicoConsultado == null)
            {
                return null;
            }
            context.Entry(medicoConsultado).CurrentValues.SetValues(medico);
            await UpdateMedicoEspecialidades(medico, medicoConsultado);
            await context.SaveChangesAsync();
            return medicoConsultado;
        }

        private async Task UpdateMedicoEspecialidades(Medico medico, Medico medicoConsultado)
        {
            medicoConsultado.Especialidades.Clear();
            foreach (var especialidade in medico.Especialidades)
            {
                var especialidadeConsultada = await context.Especialidades.FindAsync(especialidade.Id);
                medicoConsultado.Especialidades.Add(especialidadeConsultada);
            }
        }

        public async Task DeleteMedicoAsync(int id)
        {
            var medicoConsultado = await context.Medicos.FindAsync(id);
            context.Medicos.Remove(medicoConsultado);
            await context.SaveChangesAsync();
        }
    }
}