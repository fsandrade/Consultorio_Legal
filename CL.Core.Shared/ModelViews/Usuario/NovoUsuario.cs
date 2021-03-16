using System.Collections.Generic;

namespace CL.Core.Shared.ModelViews.Usuario
{
    public class NovoUsuario
    {
        public string Login { get; set; }
        public string Senha { get; set; }

        public ICollection<ReferenciaFuncao> Funcoes { get; set; }
    }
}