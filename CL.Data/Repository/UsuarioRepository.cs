namespace CL.Data.Repository;

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
        return await context.Usuarios
            .Include(p => p.Funcoes)
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Login == login);
    }

    public async Task<Usuario> InsertAsync(Usuario usuario)
    {
        await InsertUsuarioFuncaoAsync(usuario);
        await context.Usuarios.AddAsync(usuario);
        await context.SaveChangesAsync();
        return usuario;
    }

    private async Task InsertUsuarioFuncaoAsync(Usuario usuario)
    {
        var funcoesConsultas = new List<Funcao>();
        foreach (var funcao in usuario.Funcoes)
        {
            var funcaoConsultada = await context.Funcoes.FindAsync(funcao.Id);
            funcoesConsultas.Add(funcaoConsultada);
        }
        usuario.Funcoes = funcoesConsultas;
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