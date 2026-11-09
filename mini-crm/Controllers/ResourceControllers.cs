using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.UseCases.Shared;
using mini_crm.Domain.Entities;

namespace mini_crm.Controllers;

[ApiController]
[Route("api/clientes")]
public sealed class ClientesController(GenericCrudHandler<Cliente> handler) : EntityController<Cliente>(handler) { }

[ApiController]
[Route("api/influencers")]
public sealed class InfluencersController(GenericCrudHandler<Influencer> handler) : EntityController<Influencer>(handler) { }

[ApiController]
[Route("api/redes-sociais")]
public sealed class RedesSociaisController(GenericCrudHandler<RedeSocial> handler) : EntityController<RedeSocial>(handler) { }

[ApiController]
[Route("api/influencer-redes-sociais")]
public sealed class InfluencerRedesSociaisController(GenericCrudHandler<InfluencerRedeSocial> handler) : EntityController<InfluencerRedeSocial>(handler) { }

[ApiController]
[Route("api/agencias")]
public sealed class AgenciasController(GenericCrudHandler<Agencia> handler) : EntityController<Agencia>(handler) { }

[ApiController]
[Route("api/agentes")]
public sealed class AgentesController(GenericCrudHandler<Agente> handler) : EntityController<Agente>(handler) { }

[ApiController]
[Route("api/agencia-agentes")]
public sealed class AgenciaAgentesController(GenericCrudHandler<AgenciaAgente> handler) : EntityController<AgenciaAgente>(handler) { }

[ApiController]
[Route("api/intermediarios")]
public sealed class IntermediariosController(GenericCrudHandler<Intermediario> handler) : EntityController<Intermediario>(handler) { }

[ApiController]
[Route("api/bancos")]
public sealed class BancosController(GenericCrudHandler<Banco> handler) : EntityController<Banco>(handler) { }

[ApiController]
[Route("api/contas-bancarias")]
public sealed class ContasBancariasController(GenericCrudHandler<ContaBancaria> handler) : EntityController<ContaBancaria>(handler) { }

[ApiController]
[Route("api/influencer-contas-bancarias")]
public sealed class InfluencerContasBancariasController(GenericCrudHandler<InfluencerContaBancaria> handler) : EntityController<InfluencerContaBancaria>(handler) { }

[ApiController]
[Route("api/contratos")]
public sealed class ContratosController(GenericCrudHandler<Contrato> handler) : EntityController<Contrato>(handler) { }

[ApiController]
[Route("api/despesas")]
public sealed class DespesasController(GenericCrudHandler<Despesa> handler) : EntityController<Despesa>(handler) { }

[ApiController]
[Route("api/pagamentos")]
public sealed class PagamentosController(GenericCrudHandler<Pagamento> handler) : EntityController<Pagamento>(handler) { }

[ApiController]
[Route("api/notas-fiscais")]
public sealed class NotasFiscaisController(GenericCrudHandler<NotaFiscal> handler) : EntityController<NotaFiscal>(handler) { }

[ApiController]
[Route("api/formatos-nota-fiscal")]
public sealed class FormatosNotaFiscalController(GenericCrudHandler<FormatoNotaFiscal> handler) : EntityController<FormatoNotaFiscal>(handler) { }

[ApiController]
[Route("api/status-nota-fiscal")]
public sealed class StatusNotasFiscaisController(GenericCrudHandler<StatusNotaFiscal> handler) : EntityController<StatusNotaFiscal>(handler) { }

[ApiController]
[Route("api/agendamentos-nota-fiscal")]
public sealed class AgendamentosNotaFiscalController(GenericCrudHandler<AgendamentoNotaFiscal> handler) : EntityController<AgendamentoNotaFiscal>(handler) { }
