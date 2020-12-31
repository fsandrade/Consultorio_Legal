using AutoMapper;
using CL.Core.Domain;
using CL.Core.Shared.ModelViews.Usuario;
using CL.Manager.Interfaces.Managers;
using CL.Manager.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CL.Manager.Implementation
{
    public class UsuarioManager : IUsuarioManager
    {
        private readonly IUsuarioRepository repository;
        private readonly IMapper mapper;

        public UsuarioManager(IUsuarioRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UsuarioView>> GetAsync()
        {
            return mapper.Map<IEnumerable<Usuario>, IEnumerable<UsuarioView>>(await repository.GetAsync());
        }

        public async Task<UsuarioView> GetAsync(string login)
        {
            return mapper.Map<UsuarioView>(await repository.GetAsync(login));
        }

        public async Task<UsuarioView> InsertAsync(Usuario usuario)
        {
            ConverteSenhaEmHash(usuario);
            return mapper.Map<UsuarioView>(await repository.InsertAsync(usuario));
        }

        private void ConverteSenhaEmHash(Usuario usuario)
        {
            var passwordHasher = new PasswordHasher<Usuario>();
            usuario.Senha = passwordHasher.HashPassword(usuario, usuario.Senha);
        }

        public async Task<UsuarioView> UpdateMedicoAsync(Usuario usuario)
        {
            ConverteSenhaEmHash(usuario);
            return mapper.Map<UsuarioView>(await repository.UpdateAsync(usuario));
        }

        public async Task<bool> ValidaSenhaAsync(Usuario usuario)
        {
            var usuarioConsultado = await repository.GetAsync(usuario.Login);
            if (usuarioConsultado == null)
            {
                return false;
            }
            return await ValidaEAtualizaHashAsync(usuario, usuarioConsultado.Senha);
        }

        private async Task<bool> ValidaEAtualizaHashAsync(Usuario usuario, string hash)
        {
            var passwordHasher = new PasswordHasher<Usuario>();
            var status = passwordHasher.VerifyHashedPassword(usuario, hash, usuario.Senha);
            switch (status)
            {
                case PasswordVerificationResult.Failed:
                    return false;

                case PasswordVerificationResult.Success:
                    return true;

                case PasswordVerificationResult.SuccessRehashNeeded:
                    await UpdateMedicoAsync(usuario);
                    return true;

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}