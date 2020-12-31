using CL.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CL.Manager.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAsync();

        Task<Usuario> GetAsync(string login);

        Task<Usuario> InsertAsync(Usuario usuario);

        Task<Usuario> UpdateAsync(Usuario usuario);
    }
}