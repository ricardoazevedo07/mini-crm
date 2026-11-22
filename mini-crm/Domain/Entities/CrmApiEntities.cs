namespace mini_crm.Domain.Entities;

public class Lead
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Empresa { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public long? ResponsavelId { get; set; }
    public string Status { get; set; } = "Novo";
}

public class Proposta
{
    public long Id { get; set; }
    public long OportunidadeId { get; set; }
    public string Status { get; set; } = "Rascunho";
    public int Versao { get; set; } = 1;
    public Dinheiro ValorTotal { get; set; }
    public ICollection<ItemProposta> Itens { get; set; } = new List<ItemProposta>();
}

public class ItemProposta
{
    public long Id { get; set; }
    public long PropostaId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public Dinheiro ValorUnitario { get; set; }
}

public class MarcoCampanha
{
    public long Id { get; set; }
    public long CampanhaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataLimiteUtc { get; set; }
    public bool Concluido { get; set; }
}

public class SolicitacaoCuradoria
{
    public Guid Id { get; set; }
    public long CampanhaId { get; set; }
    public string Status { get; set; } = "Nova";
    public string Briefing { get; set; } = string.Empty;
}

public class CandidatoCuradoria
{
    public Guid Id { get; set; }
    public Guid SolicitacaoCuradoriaId { get; set; }
    public long InfluencerEntidadeComercialId { get; set; }
    public string Status { get; set; } = "Pendente";
    public decimal? Avaliacao { get; set; }
}

public class TarefaCrm
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public long? ResponsavelId { get; set; }
    public DateTime? PrazoUtc { get; set; }
    public string Status { get; set; } = "Pendente";
}

public class NotificacaoCrm
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public bool Lida { get; set; }
    public DateTime CriadaEmUtc { get; set; } = DateTime.UtcNow;
}
