using CL.Core.Domain;
using CL.Data.Context;
using CL.Manager.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Data.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ClContext context;

        public UsuarioRepository(ClContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAsync()
        {
            return await context.Usuarios.AsNoTracking().ToListAsync();
        }

        public async Task<Usuario> GetAsync(string login)
        {
            return await context.Usuarios.AsNoTracking()
                .SingleOrDefaultAsync(p => p.Login == login);
        }

        public async Task<Usuario> InsertAsync(Usuario usuario)
        {
            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> UpdateAsync(Usuario usuario)
        {
            var usuarioConsultado = await context.Usuarios.FindAsync(usuario.Login);
            if (usuarioConsultado == null)
            {
                return null;
            }
            context.Entry(usuarioConsultado).CurrentValues.SetValues(usuario);
            await context.SaveChangesAsync();
            return usuarioConsultado;
        }
    }
}