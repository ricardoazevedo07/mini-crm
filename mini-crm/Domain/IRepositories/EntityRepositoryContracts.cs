using mini_crm.Domain.Entities;

namespace mini_crm.Domain.IRepositories;

public interface IEntidadeComercialRepository : IRepository<EntidadeComercial>, IPagedRepository<EntidadeComercial> { }
public interface IClienteRepository : IRepository<Cliente>, IPagedRepository<Cliente> { }
public interface IInfluencerRepository : IRepository<Influencer>, IPagedRepository<Influencer> { }
public interface IRedeSocialRepository : IRepository<RedeSocial>, IPagedRepository<RedeSocial> { }
public interface IInfluencerRedeSocialRepository : IRepository<InfluencerRedeSocial>, IPagedRepository<InfluencerRedeSocial> { }
public interface IAgenciaRepository : IRepository<Agencia>, IPagedRepository<Agencia> { }
public interface IAgenteRepository : IRepository<Agente>, IPagedRepository<Agente> { }
public interface IAgenciaAgenteRepository : IRepository<AgenciaAgente>, IPagedRepository<AgenciaAgente> { }
public interface IIntermediarioRepository : IRepository<Intermediario>, IPagedRepository<Intermediario> { }
public interface IBancoRepository : IRepository<Banco>, IPagedRepository<Banco> { }
public interface IContaBancariaRepository : IRepository<ContaBancaria>, IPagedRepository<ContaBancaria> { }
public interface IInfluencerContaBancariaRepository : IRepository<InfluencerContaBancaria>, IPagedRepository<InfluencerContaBancaria> { }
public interface IContratoRepository : IRepository<Contrato>, IPagedRepository<Contrato> { }
public interface IDespesaRepository : IRepository<Despesa>, IPagedRepository<Despesa> { }
public interface IPagamentoRepository : IRepository<Pagamento>, IPagedRepository<Pagamento> { }
public interface INotaFiscalRepository : IRepository<NotaFiscal>, IPagedRepository<NotaFiscal> { }
public interface IFormatoNotaFiscalRepository : IRepository<FormatoNotaFiscal>, IPagedRepository<FormatoNotaFiscal> { }
public interface IStatusNotaFiscalRepository : IRepository<StatusNotaFiscal>, IPagedRepository<StatusNotaFiscal> { }
public interface IAgendamentoNotaFiscalRepository : IRepository<AgendamentoNotaFiscal>, IPagedRepository<AgendamentoNotaFiscal> { }
