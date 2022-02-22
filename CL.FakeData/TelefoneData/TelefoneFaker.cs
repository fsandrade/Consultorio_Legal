using Bogus;
using CL.Core.Domain;

namespace CL.FakeData.TelefoneData
{
    public class TelefoneFaker : Faker<Telefone>
    {
        public TelefoneFaker(int clientId)
        {
            RuleFor(o => o.ClienteId, _ => clientId);
            RuleFor(o => o.Numero, f => f.Person.Phone);
        }
    }
}