using Bogus;
using CL.Core.Domain;

namespace CL.FakeData.MedicoData
{
    public class MedicoFaker : Faker<Medico>
    {
        public MedicoFaker()
        {
            var id = new Faker().Random.Number(1, 999999);
            RuleFor(r => r.Id, _ => id);
            RuleFor(r => r.Nome, f => f.Person.FullName);
            RuleFor(r => r.CRM, f => f.Random.Number(1, 9999));
        }
    }
}