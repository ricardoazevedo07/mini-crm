namespace mini_crm.Domain.Entities;

public enum Divisao
{
    AVista = 1
}

public enum Moeda
{
    Real = 1,
    Dolar = 2
}

public enum StatusOportunidade
{
    Nova = 1,
    Qualificada = 2,
    Proposta = 3,
    Ganha = 4,
    Perdida = 5,
    Fechada = 6
}

public enum StatusCampanha
{
    Planejada = 1,
    EmExecucao = 2,
    EmAprovacao = 3,
    Concluida = 4,
    Cancelada = 5
}

public enum StatusParticipacao
{
    Convidado = 1,
    Negotiado = 2,
    Aprovado = 3,
    Ativo = 4,
    Cancelado = 5,
    Concluido = 6
}

public enum StatusEntrega
{
    Pendente = 1,
    EmProducao = 2,
    EmAprovacao = 3,
    Rejeitada = 4,
    Aprovada = 5,
    Publicada = 6
}

public enum StatusContratoCrm
{
    Rascunho = 1,
    EmRevisao = 2,
    Assinado = 3,
    Cancelado = 4
}

public enum StatusObrigacao
{
    Pendente = 1,
    Parcial = 2,
    Liquidada = 3,
    Cancelada = 4
}
