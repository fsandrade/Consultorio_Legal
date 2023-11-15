using CL.Core.Shared.Extensions;

namespace CL.Data.Repository;

public class MedicoRepository : IMedicoRepository
{
    private readonly ClContext context;

    public MedicoRepository(ClContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Medico>> GetMedicosAsync()
    {
        return await context.Medicos
          .Include(p => p.Especialidades)
          .AsNoTracking().ToListAsync();
    }

    public async Task<Medico> GetMedicoAsync(int id)
    {
        return await context.Medicos
            .Include(p => p.Especialidades)
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Medico> InsertMedicoAsync(Medico medico)
    {
        await InsertMedicoEspecilidades(medico);
        await context.Medicos.AddAsync(medico);
        await context.SaveChangesAsync();
        return medico;
    }

    private async Task InsertMedicoEspecilidades(Medico medico)
    {
        var especialidadesConsultadas = new List<Especialidade>();
        foreach (var especialidade in medico.Especialidades)
        {
            var especialidadeConsultada = await context.Especialidades.FindAsync(especialidade.Id);
            especialidadesConsultadas.Add(especialidadeConsultada);
        }
        medico.Especialidades = especialidadesConsultadas;
    }

    public async Task<Medico> UpdateMedicoAsync(Medico medico)
    {
        var medicoConsultado = await context.Medicos
                                    .Include(p => p.Especialidades)
                                    .SingleOrDefaultAsync(p => p.Id == medico.Id);
        if (medicoConsultado == null)
        {
            return null;
        }
        context.Entry(medicoConsultado).CurrentValues.SetValues(medico);
        await UpdateMedicoEspecialidades(medico, medicoConsultado);
        await context.SaveChangesAsync();
        return medicoConsultado;
    }

    private async Task UpdateMedicoEspecialidades(Medico medico, Medico medicoConsultado)
    {
        medicoConsultado.Especialidades.RemoveAll(p => !medico.Especialidades.Contains(p));
        var especialidadesAdicionadas = medico.Especialidades.Except(medicoConsultado.Especialidades).Select(p => p.Id);
        var especialidadesConsultadas = await context.Especialidades.Where(p => especialidadesAdicionadas.Contains(p.Id)).ToListAsync();
        medicoConsultado.Especialidades.AddRange(especialidadesConsultadas);
    }

    public async Task<Medico> DeleteMedicoAsync(int id)
    {
        var medicoConsultado = await context.Medicos.FindAsync(id);
        if (medicoConsultado == null)
        {
            return null;
        }
        var medicoRemovido = context.Medicos.Remove(medicoConsultado);
        await context.SaveChangesAsync();
        return medicoRemovido.Entity;
    }
}