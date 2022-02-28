using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CL.Data.Configuration;

public class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        builder.HasKey(p => p.ClienteId);
        builder.Property(p => p.Estado).HasConversion(
            p => p.ToString(),
            p => (Estado)Enum.Parse(typeof(Estado), p));
    }
}