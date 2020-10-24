using CL.Data.Context;
using CL.Manager.Interfaces.Repositories;
using System.Threading.Tasks;

namespace CL.Data.Repository
{
    public class EspecialidadeRepository : IEspecialidadeRepository
    {
        private readonly ClContext context;

        public EspecialidadeRepository(ClContext context)
        {
            this.context = context;
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await context.Especialidades.FindAsync(id) != null;
        }
    }
}