using Microsoft.EntityFrameworkCore;
using mini_crm.Domain.Entities;

namespace mini_crm.Infrastructure.DbContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<EntidadeComercial> EntidadesComerciais => Set<EntidadeComercial>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Influencer> Influencers => Set<Influencer>();
    public DbSet<RedeSocial> RedesSociais => Set<RedeSocial>();
    public DbSet<InfluencerRedeSocial> InfluencerRedesSociais => Set<InfluencerRedeSocial>();
    public DbSet<Agencia> Agencias => Set<Agencia>();
    public DbSet<Agente> Agentes => Set<Agente>();
    public DbSet<AgenciaAgente> AgenciaAgentes => Set<AgenciaAgente>();
    public DbSet<Intermediario> Intermediarios => Set<Intermediario>();
    public DbSet<Banco> Bancos => Set<Banco>();
    public DbSet<ContaBancaria> ContasBancarias => Set<ContaBancaria>();
    public DbSet<InfluencerContaBancaria> InfluencerContasBancarias => Set<InfluencerContaBancaria>();
    public DbSet<Contrato> Contratos => Set<Contrato>();
    public DbSet<Despesa> Despesas => Set<Despesa>();
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();
    public DbSet<NotaFiscal> NotasFiscais => Set<NotaFiscal>();
    public DbSet<FormatoNotaFiscal> FormatosNotaFiscal => Set<FormatoNotaFiscal>();
    public DbSet<StatusNotaFiscal> StatusNotasFiscais => Set<StatusNotaFiscal>();
    public DbSet<AgendamentoNotaFiscal> AgendamentosNotaFiscal => Set<AgendamentoNotaFiscal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EntidadeComercial>(entity =>
        {
            entity.HasKey(x => x.EntidadeComercialId);
            entity.HasDiscriminator<string>("Tipo")
                .HasValue<Cliente>("Cliente")
                .HasValue<Influencer>("Influencer")
                .HasValue<Agencia>("Agencia")
                .HasValue<Intermediario>("Intermediario");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.Codigo).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Influencer>(entity =>
        {
            entity.Property(x => x.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Agencia>(entity =>
        {
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.Nome).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Intermediario>(entity =>
        {
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.Nome).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<RedeSocial>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nome).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<InfluencerRedeSocial>(entity =>
        {
            entity.HasKey(x => new { x.InfluencerEntidadeComercialId, x.RedeSocialId });
            entity.Property(x => x.Link).HasMaxLength(500).IsRequired();
            entity.HasOne(x => x.Influencer)
                .WithMany(x => x.RedesSociais)
                .HasForeignKey(x => x.InfluencerEntidadeComercialId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.RedeSocial)
                .WithMany(x => x.Influencers)
                .HasForeignKey(x => x.RedeSocialId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Agente>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nome).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Contato).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<AgenciaAgente>(entity =>
        {
            entity.HasKey(x => new { x.AgenciaEntidadeComercialId, x.AgenteId });
            entity.HasOne(x => x.Agencia)
                .WithMany(x => x.Agentes)
                .HasForeignKey(x => x.AgenciaEntidadeComercialId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.Agente)
                .WithMany(x => x.Agencias)
                .HasForeignKey(x => x.AgenteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Banco>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nome).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<ContaBancaria>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Agencia).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Conta).HasMaxLength(50).IsRequired();
            entity.HasOne(x => x.Banco)
                .WithMany(x => x.ContasBancarias)
                .HasForeignKey(x => x.BancoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InfluencerContaBancaria>(entity =>
        {
            entity.HasKey(x => new { x.InfluencerEntidadeComercialId, x.ContaBancariaId });
            entity.HasOne(x => x.Influencer)
                .WithMany(x => x.ContasBancarias)
                .HasForeignKey(x => x.InfluencerEntidadeComercialId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.ContaBancaria)
                .WithMany(x => x.Influencers)
                .HasForeignKey(x => x.ContaBancariaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Contrato>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Divisao).HasConversion<string>().HasMaxLength(50).IsRequired();
            entity.Property(x => x.Moeda).HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.HasOne(x => x.Cliente)
                .WithMany(x => x.Contratos)
                .HasForeignKey(x => x.ClienteEntidadeComercialId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.AgenciaAgente)
                .WithMany(x => x.Contratos)
                .HasForeignKey(x => new { x.AgenciaEntidadeComercialId, x.AgenteId })
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Influencer)
                .WithMany(x => x.Contratos)
                .HasForeignKey(x => x.InfluencerEntidadeComercialId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Despesa>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Valor).HasPrecision(18, 2);
            entity.Property(x => x.Observacao).HasMaxLength(1000);
            entity.HasOne(x => x.Contrato)
                .WithMany(x => x.Despesas)
                .HasForeignKey(x => x.ContratoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Pagamento>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Valor).HasPrecision(18, 2);
            entity.HasOne(x => x.Despesa)
                .WithMany(x => x.Pagamentos)
                .HasForeignKey(x => x.DespesaId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(x => x.Contrato)
                .WithMany(x => x.Pagamentos)
                .HasForeignKey(x => x.ContratoId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(x => x.EntidadeComercial)
                .WithMany(x => x.Pagamentos)
                .HasForeignKey(x => x.EntidadeComercialId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.NotaFiscal)
                .WithMany(x => x.Pagamentos)
                .HasForeignKey(x => x.NotaFiscalId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FormatoNotaFiscal>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nome).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<NotaFiscal>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.RazaoSocial).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Valor).HasPrecision(18, 2);
            entity.HasOne(x => x.FormatoNotaFiscal)
                .WithMany(x => x.NotasFiscais)
                .HasForeignKey(x => x.FormatoNotaFiscalId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StatusNotaFiscal>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nome).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<AgendamentoNotaFiscal>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.DataPrazoRepasseObservacao).HasMaxLength(1000);
            entity.HasOne(x => x.NotaFiscal)
                .WithMany(x => x.Agendamentos)
                .HasForeignKey(x => x.NotaFiscalId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.StatusNotaFiscal)
                .WithMany(x => x.Agendamentos)
                .HasForeignKey(x => x.StatusNotaFiscalId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
