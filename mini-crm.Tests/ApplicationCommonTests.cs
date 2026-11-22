using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using mini_crm.Application.Common;
using mini_crm.Domain.Entities;
using mini_crm.Infrastructure.DbContext;
using Microting.EntityFrameworkCore.MySql.Infrastructure;
using Xunit;

namespace mini_crm.Tests;

public class ApplicationCommonTests
{
    [Fact]
    public void FailurePreservesApplicationError()
    {
        var error = ApplicationErrors.NotFound("Influencer not found");
        var result = OperationResult<string>.Failure(error);

        Assert.False(result.Succeeded);
        Assert.Null(result.Value);
        Assert.Equal(404, result.Error!.StatusCode);
    }

    [Fact]
    public void PageRequestAcceptsValidBounds()
    {
        var request = new PageRequest(1, 100);

        request.Validate();

        Assert.Equal(1, request.Page);
        Assert.Equal(100, request.PageSize);
    }

    [Fact]
    public void PagedResultCalculatesTotalPages()
    {
        var result = new PagedResult<int>([1, 2], 1, 2, 5);

        Assert.Equal(3, result.TotalPages);
    }

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 0)]
    [InlineData(1, 101)]
    public void PageRequestRejeitaValoresInvalidos(int page, int pageSize)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PageRequest(page, pageSize).Validate());
    }

    [Fact]
    public void ClienteNormalizaCnpj()
    {
        var cliente = new Cliente();

        cliente.AtualizarDadosComerciais("Empresa", "12.345.678/0001-90", "Marketing", "pt-BR");

        Assert.Equal("12345678000190", cliente.Cnpj);
        Assert.Equal("Empresa", cliente.Nome);
        Assert.True(cliente.Ativo);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("12.345.678/0001-9")]
    public void ClienteRejeitaCnpjInvalido(string cnpj)
    {
        var cliente = new Cliente();

        Assert.Throws<InvalidOperationException>(() =>
            cliente.AtualizarDadosComerciais("Empresa", cnpj, "Marketing", "pt-BR"));
    }

    [Fact]
    public void ClientePodeSerArquivadoEReativado()
    {
        var cliente = new Cliente();

        cliente.Arquivar();
        Assert.False(cliente.Ativo);

        cliente.Reativar();
        Assert.True(cliente.Ativo);
    }

    [Fact]
    public void ContatoClientePodeSerAtualizadoIndependentemente()
    {
        var cliente = new Cliente { Id = Guid.NewGuid() };
        var contato = new ContatoCliente { Id = Guid.NewGuid(), Cliente = cliente };

        contato.Atualizar("Ana", "Diretora", "ana@example.com", "+55 11 99999-9999", true);

        Assert.Equal("Ana", contato.Nome);
        Assert.True(contato.Principal);
        Assert.Equal(cliente.Id, contato.Cliente!.Id);
    }

    [Fact]
    public void ContatoClienteRejeitaDadosObrigatoriosAusentes()
    {
        var contato = new ContatoCliente();

        Assert.Throws<InvalidOperationException>(() => contato.Atualizar("", null, "", null, false));
    }

    [Fact]
    public void DinheiroNormalizaMoedaEValor()
    {
        var dinheiro = new Dinheiro(10.5m, "brl");

        Assert.Equal(10.50m, dinheiro.Valor);
        Assert.Equal("BRL", dinheiro.CodigoMoeda);
    }

    [Theory]
    [InlineData(-1, "BRL")]
    [InlineData(10, "R")]
    [InlineData(10, "12A")]
    public void DinheiroRejeitaValoresInvalidos(decimal valor, string moeda)
    {
        Assert.Throws<InvalidOperationException>(() => new Dinheiro(valor, moeda));
    }

    [Fact]
    public void DinheiroNaoPermiteOperacaoEntreMoedasDiferentes()
    {
        var reais = new Dinheiro(10m, "BRL");
        var dolares = new Dinheiro(10m, "USD");

        Assert.Throws<InvalidOperationException>(() => reais.Somar(dolares));
    }

    [Fact]
    public void ModeloEfMantemClienteEContatoComChavesCompatíveis()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql("Server=localhost;Database=test;User=test;Password=test;", new MySqlServerVersion(new Version(8, 0, 0)))
            .Options;

        using var context = new AppDbContext(options);
        var cliente = context.Model.FindEntityType(typeof(Cliente));
        var contato = context.Model.FindEntityType(typeof(ContatoCliente));

        Assert.NotNull(cliente);
        Assert.NotNull(contato);
        Assert.Equal("EntidadeComercialId", cliente!.FindPrimaryKey()!.Properties.Single().Name);
        Assert.Contains(cliente.GetIndexes(), index => index.Properties.Any(property => property.Name == "Cnpj") && index.IsUnique);
        Assert.NotNull(contato!.FindNavigation(nameof(ContatoCliente.Cliente)));
    }

    [Fact]
    public void OportunidadeSegueStatusEConversaoUnica()
    {
        var oportunidade = new Oportunidade { ClienteEntidadeComercialId = 10, ResponsavelId = 20 };

        oportunidade.Qualificar("Briefing", new Dinheiro(10000m, "BRL"));
        oportunidade.RegistrarProposta(new Dinheiro(12000m, "BRL"));
        oportunidade.MarcarGanha();
        oportunidade.VincularCampanha(42);

        Assert.Equal(StatusOportunidade.Fechada, oportunidade.Status);
        Assert.Equal(42, oportunidade.CampanhaId);
        Assert.Throws<InvalidOperationException>(() => oportunidade.VincularCampanha(43));
    }

    [Fact]
    public void OportunidadeExigeMotivoNaPerda()
    {
        var oportunidade = new Oportunidade();

        Assert.Throws<InvalidOperationException>(() => oportunidade.Perder("  "));
    }

    [Fact]
    public void CampanhaNaoFechaComSaldoPendente()
    {
        var campanha = new Campanha();
        campanha.Configurar("Influencer", DateTime.UtcNow, null, new Dinheiro(50000m, "BRL"), TipoTaxa.Percentual, 15m);
        campanha.Iniciar();
        campanha.EnviarParaAprovacao();

        Assert.Throws<InvalidOperationException>(() => campanha.Concluir(true, new Dinheiro(100m, "BRL")));

        campanha.Concluir(true, new Dinheiro(0m, "BRL"));
        Assert.Equal(StatusCampanha.Concluida, campanha.Status);
    }

    [Fact]
    public void ContaReceberCalculaLiquidacaoParcial()
    {
        var conta = new ContaReceber();
        conta.Configurar(new Dinheiro(1000m, "BRL"), new Dinheiro(800m, "BRL"), DateTime.UtcNow.AddDays(10));

        conta.RegistrarRecebimento(new Dinheiro(300m, "BRL"));

        Assert.Equal(StatusObrigacao.Parcial, conta.Status);
        Assert.Equal(500m, conta.Saldo.Valor);

        conta.RegistrarRecebimento(new Dinheiro(500m, "BRL"));
        Assert.Equal(StatusObrigacao.Liquidada, conta.Status);
        Assert.Throws<InvalidOperationException>(() => conta.RegistrarRecebimento(new Dinheiro(1m, "BRL")));
    }

    [Fact]
    public void EntregaPublicacaoExigeAprovacaoEEvidencia()
    {
        var entrega = new EntregaCampanha();
        entrega.Configurar("Vídeo", DateTime.UtcNow.AddDays(5));

        entrega.Iniciar();
        entrega.EnviarParaAprovacao();
        Assert.Throws<InvalidOperationException>(() => entrega.Publicar("", DateTime.UtcNow));

        entrega.Aprovar();
        entrega.Publicar("https://example.com/post", DateTime.UtcNow);
        Assert.Equal(StatusEntrega.Publicada, entrega.Status);
    }

    [Fact]
    public void ContratoAssinadoPreservaVersaoAoAmendar()
    {
        var contrato = new Contrato { Id = 10, ConteudoContrato = "termos iniciais" };
        contrato.Assinar(DateTime.UtcNow);
        Assert.Throws<InvalidOperationException>(() => contrato.Cancelar("desistência"));

        contrato.CriarVersao(DateTime.UtcNow);

        Assert.Equal(2, contrato.Versao);
        Assert.Single(contrato.Versoes);
        Assert.Equal("termos iniciais", contrato.Versoes.Single().ConteudoSnapshot);
        Assert.Equal(StatusContratoCrm.EmRevisao, contrato.StatusCrm);
    }

    [Fact]
    public void ParticipacaoAprovadaNaoAceitaCompensacaoExcedente()
    {
        var participacao = new ParticipacaoCampanha();
        participacao.Negociar(new Dinheiro(500m, "BRL"), "Entrega de dois vídeos");
        participacao.Aprovar(new Dinheiro(500m, "BRL"));
        participacao.Ativar();

        Assert.Equal(StatusParticipacao.Ativo, participacao.Status);
    }
}
