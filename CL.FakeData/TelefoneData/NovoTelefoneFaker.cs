using Bogus;
using CL.Core.Shared.ModelViews.Telefone;

namespace CL.FakeData.TelefoneData
{
    public class NovoTelefoneFaker : Faker<NovoTelefone>
    {
        public NovoTelefoneFaker()
        {
            RuleFor(p => p.Numero, f => f.Person.Phone);
        }
    }
}