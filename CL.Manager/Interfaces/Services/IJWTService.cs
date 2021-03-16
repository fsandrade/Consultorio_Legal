using CL.Core.Domain;

namespace CL.Manager.Interfaces.Services
{
    public interface IJWTService
    {
        string GerarToken(Usuario usuario);
    }
}