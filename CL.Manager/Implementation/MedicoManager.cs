using CL.Core.Shared.ModelViews.Medico;

namespace CL.Manager.Implementation;

public class MedicoManager : IMedicoManager
{
    private readonly IMedicoRepository repository;
    private readonly IMapper mapper;

    public MedicoManager(IMedicoRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<MedicoView>> GetMedicosAsync()
    {
        return mapper.Map<IEnumerable<Medico>, IEnumerable<MedicoView>>(await repository.GetMedicosAsync());
    }

    public async Task<MedicoView> GetMedicoAsync(int id)
    {
        return mapper.Map<MedicoView>(await repository.GetMedicoAsync(id));
    }

    public async Task<MedicoView> InsertMedicoAsync(NovoMedico novoMedico)
    {
        var medico = mapper.Map<Medico>(novoMedico);
        return mapper.Map<MedicoView>(await repository.InsertMedicoAsync(medico));
    }

    public async Task<MedicoView> UpdateMedicoAsync(AlteraMedico alteraMedico)
    {
        var medico = mapper.Map<Medico>(alteraMedico);
        return mapper.Map<MedicoView>(await repository.UpdateMedicoAsync(medico));
    }

    public async Task DeleteMedicoAsync(int id)
    {
        await repository.DeleteMedicoAsync(id);
    }
}