namespace CL.Core.Shared.ModelViews.Usuario;

public class UsuarioView
{
    public string Login { get; set; }

    public ICollection<FuncaoView> Funcoes { get; set; }
}