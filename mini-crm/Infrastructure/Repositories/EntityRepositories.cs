using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;
using mini_crm.Infrastructure.DbContext;

namespace mini_crm.Infrastructure.Repositories;

public sealed class EntidadeComercialRepository(AppDbContext context) : EfRepository<EntidadeComercial>(context), IEntidadeComercialRepository { }
public sealed class ClienteRepository(AppDbContext context) : EfRepository<Cliente>(context), IClienteRepository { }
public sealed class InfluencerRepository(AppDbContext context) : EfRepository<Influencer>(context), IInfluencerRepository { }
public sealed class RedeSocialRepository(AppDbContext context) : EfRepository<RedeSocial>(context), IRedeSocialRepository { }
public sealed class InfluencerRedeSocialRepository(AppDbContext context) : EfRepository<InfluencerRedeSocial>(context), IInfluencerRedeSocialRepository { }
public sealed class AgenciaRepository(AppDbContext context) : EfRepository<Agencia>(context), IAgenciaRepository { }
public sealed class AgenteRepository(AppDbContext context) : EfRepository<Agente>(context), IAgenteRepository { }
public sealed class AgenciaAgenteRepository(AppDbContext context) : EfRepository<AgenciaAgente>(context), IAgenciaAgenteRepository { }
public sealed class IntermediarioRepository(AppDbContext context) : EfRepository<Intermediario>(context), IIntermediarioRepository { }
public sealed class BancoRepository(AppDbContext context) : EfRepository<Banco>(context), IBancoRepository { }
public sealed class ContaBancariaRepository(AppDbContext context) : EfRepository<ContaBancaria>(context), IContaBancariaRepository { }
public sealed class InfluencerContaBancariaRepository(AppDbContext context) : EfRepository<InfluencerContaBancaria>(context), IInfluencerContaBancariaRepository { }
public sealed class ContratoRepository(AppDbContext context) : EfRepository<Contrato>(context), IContratoRepository { }
public sealed class DespesaRepository(AppDbContext context) : EfRepository<Despesa>(context), IDespesaRepository { }
public sealed class PagamentoRepository(AppDbContext context) : EfRepository<Pagamento>(context), IPagamentoRepository { }
public sealed class NotaFiscalRepository(AppDbContext context) : EfRepository<NotaFiscal>(context), INotaFiscalRepository { }
public sealed class FormatoNotaFiscalRepository(AppDbContext context) : EfRepository<FormatoNotaFiscal>(context), IFormatoNotaFiscalRepository { }
public sealed class StatusNotaFiscalRepository(AppDbContext context) : EfRepository<StatusNotaFiscal>(context), IStatusNotaFiscalRepository { }
public sealed class AgendamentoNotaFiscalRepository(AppDbContext context) : EfRepository<AgendamentoNotaFiscal>(context), IAgendamentoNotaFiscalRepository { }
