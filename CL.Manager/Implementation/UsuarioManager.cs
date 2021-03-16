using AutoMapper;
using CL.Core.Domain;
using CL.Core.Shared.ModelViews.Usuario;
using CL.Manager.Interfaces.Managers;
using CL.Manager.Interfaces.Repositories;
using CL.Manager.Interfaces.Services;
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
        private readonly IJWTService jwt;

        public UsuarioManager(IUsuarioRepository repository, IMapper mapper, IJWTService jwt)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.jwt = jwt;
        }

        public async Task<IEnumerable<UsuarioView>> GetAsync()
        {
            return mapper.Map<IEnumerable<Usuario>, IEnumerable<UsuarioView>>(await repository.GetAsync());
        }

        public async Task<UsuarioView> GetAsync(string login)
        {
            return mapper.Map<UsuarioView>(await repository.GetAsync(login));
        }

        public async Task<UsuarioView> InsertAsync(NovoUsuario novoUsuario)
        {
            var usuario = mapper.Map<Usuario>(novoUsuario);
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

        public async Task<UsuarioLogado> ValidaUsuarioEGeraTokenAsync(Usuario usuario)
        {
            var usuarioConsultado = await repository.GetAsync(usuario.Login);
            if (usuarioConsultado == null)
            {
                return null;
            }
            if (await ValidaEAtualizaHashAsync(usuario, usuarioConsultado.Senha))
            {
                var usuarioLogado = mapper.Map<UsuarioLogado>(usuarioConsultado);
                usuarioLogado.Token = jwt.GerarToken(usuarioConsultado);
                return usuarioLogado;
            }
            return null;
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