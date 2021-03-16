using System.Collections.Generic;

namespace CL.Core.Shared.ModelViews.Usuario
{
    public class UsuarioLogado
    {
        public string Login { get; set; }
        public ICollection<FuncaoView> Funcoes { get; set; }
        public string Token { get; set; }
    }
}