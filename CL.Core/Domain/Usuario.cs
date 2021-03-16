using System.Collections.Generic;

namespace CL.Core.Domain
{
    public class Usuario
    {
        public string Login { get; set; }
        public string Senha { get; set; }

        public ICollection<Funcao> Funcoes { get; set; }

        public Usuario()
        {
            Funcoes = new HashSet<Funcao>();
        }
    }
}