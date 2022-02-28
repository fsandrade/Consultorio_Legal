namespace CL.FakeData.EspecialidadeData;

public class EspecialidadeFaker : Faker<Especialidade>
{
    public EspecialidadeFaker()
    {
        RuleFor(r => r.Id, f => f.Random.Number(1, 9999999));
        RuleFor(r => r.Descricao, f => f.Random.Word());
    }
}