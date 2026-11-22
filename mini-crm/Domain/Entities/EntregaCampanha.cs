namespace mini_crm.Domain.Entities;

public class EntregaCampanha
{
    public Guid Id { get; set; }
    public long CampanhaId { get; set; }
    public Guid ParticipacaoCampanhaId { get; set; }
    public string Titulo { get; private set; } = string.Empty;
    public DateTime DataLimiteUtc { get; private set; }
    public StatusEntrega Status { get; private set; } = StatusEntrega.Pendente;
    public string? MotivoRejeicao { get; private set; }
    public string? EvidenciaPublicacao { get; private set; }
    public DateTime? PublicadoEmUtc { get; private set; }

    public void Configurar(string titulo, DateTime dataLimiteUtc)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new InvalidOperationException("O título da entrega é obrigatório.");

        Titulo = titulo.Trim();
        DataLimiteUtc = dataLimiteUtc.ToUniversalTime();
    }

    public void Iniciar() { if (Status != StatusEntrega.Pendente) throw new InvalidOperationException("Entrega precisa estar pendente."); Status = StatusEntrega.EmProducao; }

    public void EnviarParaAprovacao() { if (Status != StatusEntrega.EmProducao) throw new InvalidOperationException("Entrega precisa estar em produção."); Status = StatusEntrega.EmAprovacao; }

    public void Rejeitar(string motivo)
    {
        if (Status != StatusEntrega.EmAprovacao)
            throw new InvalidOperationException("A entrega precisa estar em aprovação.");
        if (string.IsNullOrWhiteSpace(motivo)) throw new InvalidOperationException("O motivo da rejeição é obrigatório.");
        MotivoRejeicao = motivo.Trim();
        Status = StatusEntrega.Rejeitada;
    }

    public void Aprovar() { if (Status != StatusEntrega.EmAprovacao) throw new InvalidOperationException("A entrega precisa estar em aprovação."); Status = StatusEntrega.Aprovada; }

    public void Publicar(string evidência, DateTime publicadoEmUtc)
    {
        if (Status != StatusEntrega.Aprovada)
            throw new InvalidOperationException("A entrega precisa estar aprovada.");
        if (string.IsNullOrWhiteSpace(evidência)) throw new InvalidOperationException("A evidência de publicação é obrigatória.");

        EvidenciaPublicacao = evidência.Trim();
        PublicadoEmUtc = publicadoEmUtc.ToUniversalTime();
        Status = StatusEntrega.Publicada;
    }
}
