namespace CL.Core.Domain;

public class Especialidade
{
    public int Id { get; set; }
    public string Descricao { get; set; }

    public ICollection<Medico> Medicos { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is Especialidade that)
        {
            return this.Id == that.Id;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}