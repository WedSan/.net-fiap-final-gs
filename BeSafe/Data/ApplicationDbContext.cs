using BeSafe.Models;
using Microsoft.EntityFrameworkCore;

namespace BeSafe.Data;

public class ApplicationDbContext : DbContext
{
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<RiskArea> RiskAreas { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("GS_USUARIO")
            .HasKey(u => u.Id);
            
        modelBuilder.Entity<User>()
            .Property(u => u.Id).HasColumnName("ID");
        modelBuilder.Entity<User>()
            .Property(u => u.Nome).HasColumnName("NOME").IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.Cpf).HasColumnName("CPF");
        modelBuilder.Entity<User>()
            .Property(u => u.DataNascimento).HasColumnName("DATA_NASCIMENTO").IsRequired();

        modelBuilder.Entity<Contact>()
            .ToTable("GS_CONTATO")
            .HasKey(c => c.Id);
            
        modelBuilder.Entity<Contact>()
            .Property(c => c.Id).HasColumnName("ID");
        modelBuilder.Entity<Contact>()
            .Property(c => c.Email).HasColumnName("EMAIL").IsRequired();
        modelBuilder.Entity<Contact>()
            .Property(c => c.Telefone).HasColumnName("TELEFONE").IsRequired();
        
        modelBuilder.Entity<Contact>()
            .HasOne(c => c.User)
            .WithOne(u => u.Contact)
            .HasForeignKey<Contact>(c => c.Id);
        
        modelBuilder.Entity<Address>()
            .ToTable("GS_ENDERECO_USUARIO")
            .HasKey(a => a.Id);
            
        modelBuilder.Entity<Address>()
            .Property(a => a.Id).HasColumnName("ID");
        modelBuilder.Entity<Address>()
            .Property(a => a.Logradouro).HasColumnName("LOGRADOURO").IsRequired();
        modelBuilder.Entity<Address>()
            .Property(a => a.Bairro).HasColumnName("BAIRRO").IsRequired();
        modelBuilder.Entity<Address>()
            .Property(a => a.Cidade).HasColumnName("CIDADE").IsRequired();
        modelBuilder.Entity<Address>()
            .Property(a => a.Estado).HasColumnName("ESTADO").IsRequired();
        
        modelBuilder.Entity<Address>()
            .HasOne(a => a.User)
            .WithOne(u => u.Address)
            .HasForeignKey<Address>(a => a.Id);
        
        modelBuilder.Entity<RiskArea>()
            .ToTable("GS_AREA_RISCO")
            .HasKey(r => r.Id);
            
        modelBuilder.Entity<RiskArea>()
            .Property(r => r.Id).HasColumnName("ID");
        modelBuilder.Entity<RiskArea>()
            .Property(r => r.Mensagem).HasColumnName("MENSAGEM").IsRequired();
        modelBuilder.Entity<RiskArea>()
            .Property(r => r.DataEnvio).HasColumnName("DATA_ENVIO");
        modelBuilder.Entity<RiskArea>()
            .Property(r => r.TipoAlerta).HasColumnName("TIPO_ALERTA");
        
        modelBuilder.Entity<Alert>()
            .ToTable("GS_ALERTA")
            .HasKey(a => a.Id);
            
        modelBuilder.Entity<Alert>()
            .Property(a => a.Id).HasColumnName("ID");
        modelBuilder.Entity<Alert>()
            .Property(a => a.AreaRiscoId).HasColumnName("AREA_RISCO_ID").IsRequired();
        modelBuilder.Entity<Alert>()
            .Property(a => a.Mensagem).HasColumnName("MENSAGEM").IsRequired();
        modelBuilder.Entity<Alert>()
            .Property(a => a.DataEnvio).HasColumnName("DATA_ENVIO").IsRequired();
        modelBuilder.Entity<Alert>()
            .Property(a => a.TipoAlerta).HasColumnName("TIPO_ALERTA").IsRequired();
        
        modelBuilder.Entity<Alert>()
            .HasOne(a => a.RiskArea)
            .WithMany(r => r.Alerts)
            .HasForeignKey(a => a.AreaRiscoId);
    }
}