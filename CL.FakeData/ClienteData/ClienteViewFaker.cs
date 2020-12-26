using Bogus;
using Bogus.Extensions.Brazil;
using CL.Core.Shared.ModelViews.Cliente;
using CL.FakeData.EnderecoData;
using CL.FakeData.TelefoneData;

namespace CL.FakeData.ClienteData
{
    public class ClienteViewFaker : Faker<ClienteView>
    {
        public ClienteViewFaker()
        {
            var id = new Faker().Random.Number(1, 999999);
            RuleFor(p => p.Id, f => id);
            RuleFor(p => p.Nome, f => f.Person.FullName);
            RuleFor(p => p.Sexo, f => f.PickRandom<SexoView>());
            RuleFor(p => p.Documento, f => f.Person.Cpf());
            RuleFor(p => p.Criacao, f => f.Date.Past());
            RuleFor(p => p.UltimaAtualizacao, f => f.Date.Past());
            RuleFor(p => p.DataNascimento, f => f.Date.Past());
            RuleFor(p => p.Telefones, f => new TelefoneViewFaker().Generate(3));
            RuleFor(p => p.Endereco, f => new EnderecoViewFaker().Generate());
        }
    }
}