# Tasks

## 1. Preparação e contratos compartilhados

- [x] 1.1 Definir os resultados de aplicação, `ProblemDetails`, erros de validação, recurso não encontrado e conflito, e verificar com testes unitários de conversão para respostas HTTP.
- [x] 1.2 Criar modelos compartilhados de paginação, filtros e metadados, e verificar com testes unitários para páginas vazias, primeiras páginas e limites de tamanho.
- [x] 1.3 Criar DTOs e mapeamentos para os identificadores técnicos e de negócio, verificando que `EntidadeComercialId` é usado nas rotas e os IDs específicos aparecem somente como dados de negócio.
- [ ] 1.4 Registrar handlers, services e repositories no container de DI e verificar a resolução de todas as dependências em um teste de composição da aplicação.

## 2. Repositories e infraestrutura de persistência

- [x] 2.1 Criar interfaces de repositories em `Domain/IRepositories` para entidades comerciais, catálogos, associações, contratos, financeiro e fiscal, verificando que não dependem de `AppDbContext`.
- [x] 2.2 Criar implementações assíncronas em `Infrastructure/Repositories` com consultas ordenadas, paginação e `CancellationToken`, verificando consultas de lista e busca por identificador.
- [x] 2.3 Implementar consultas específicas para validar referências de contratos, pagamentos, vínculos sociais, contas bancárias e notas fiscais, verificando os cenários de referência ausente.
- [x] 2.4 Implementar unidade de trabalho ou transações para operações que alteram múltiplos registros, verificando commit integral e rollback em caso de falha.
- [x] 2.5 Revisar o mapeamento TPH, chaves compostas, índices, precisão decimal e regras de exclusão do `AppDbContext`, gerando migration somente quando necessário e verificando-a com o banco MySQL de desenvolvimento.

## 3. Cadastros comerciais e catálogos

- [ ] 3.1 Implementar use cases e controllers CRUD para clientes, influencers, agências, intermediários, agentes e associações agência-agente, verificando criação, consulta, atualização, exclusão e `404`.
- [ ] 3.2 Implementar use cases e controllers CRUD para redes sociais e vínculos influencer-rede social, verificando criação de link, prevenção de associação duplicada e paginação.
- [ ] 3.3 Implementar validações de identificadores técnicos e de negócio, códigos, nomes, e-mails e contatos, verificando mensagens de erro por campo.
- [ ] 3.4 Adicionar testes unitários dos handlers e testes de integração dos endpoints de cadastros comerciais e catálogos, verificando os contratos documentados no Swagger.

## 4. Contas bancárias

- [ ] 4.1 Implementar use cases e controllers CRUD para bancos e contas bancárias, verificando referências válidas, atualização, exclusão segura e respostas HTTP.
- [ ] 4.2 Implementar use cases e controllers para associações influencer-conta bancária, verificando existência do influencer e da conta e rejeição de duplicidade com `409 Conflict`.
- [ ] 4.3 Adicionar testes unitários, de repository e de integração para contas bancárias e associações, verificando consultas paginadas e relacionamentos.

## 5. Contratos e despesas

- [ ] 5.1 Implementar use cases e controllers CRUD para contratos, incluindo cliente, agência-agente, influencer, divisão e moeda, verificando validação de todas as referências.
- [ ] 5.2 Implementar use cases e controllers CRUD para despesas vinculadas a contratos, verificando valores decimais, observação e impedimento de contrato inexistente.
- [ ] 5.3 Implementar regras de exclusão e consulta de contratos com despesas e pagamentos sem carregar grafos ilimitados, verificando comportamento com relacionamentos existentes.
- [ ] 5.4 Adicionar testes unitários e de integração para contratos e despesas, incluindo cenários de referências inválidas, paginação e consistência transacional.

## 6. Notas fiscais e agendamentos

- [ ] 6.1 Implementar use cases e controllers CRUD para formatos e status de nota fiscal, verificando validação de nomes e consultas paginadas.
- [ ] 6.2 Implementar use cases e controllers CRUD para notas fiscais, verificando formato existente, razão social, valor, indicador de envio e respostas DTO sem navegações do EF Core.
- [ ] 6.3 Implementar use cases e controllers para agendamentos de notas fiscais, verificando nota e status existentes, datas e observação de prazo.
- [ ] 6.4 Adicionar testes unitários, de repository e de integração para o fluxo fiscal, verificando referências inválidas, atualização e conflitos.

## 7. Pagamentos e integração financeira

- [ ] 7.1 Implementar use cases e controllers CRUD para pagamentos, verificando entidade comercial e nota fiscal obrigatórias e contrato e despesa opcionais.
- [ ] 7.2 Implementar validações de valores, datas e compatibilidade entre contrato, despesa, nota fiscal e entidade comercial, verificando rollback quando uma referência ou regra falhar.
- [ ] 7.3 Adicionar consultas paginadas e filtros por contrato, despesa, entidade comercial, nota fiscal e período, verificando que a filtragem ocorre no banco.
- [ ] 7.4 Adicionar testes unitários e de integração para pagamentos, incluindo criação com e sem contrato/despesa, relacionamentos inválidos e transação.

## 8. Controllers, Swagger e tratamento transversal

- [ ] 8.1 Garantir que todos os controllers deleguem aos handlers e não acessem `AppDbContext` diretamente, verificando a regra com revisão estrutural e testes de integração.
- [ ] 8.2 Configurar tratamento global de exceções, validação automática e respostas `ProblemDetails`, verificando `400`, `404`, `409` e `500` sanitizado.
- [ ] 8.3 Configurar tags, exemplos e schemas dos requests/responses no Swagger, verificando que todos os recursos e endpoints aparecem em `/swagger`.
- [ ] 8.4 Remover ou isolar o `WeatherForecastController` de exemplo, verificando que somente endpoints de negócio são publicados.

## 9. Verificação integrada

- [ ] 9.1 Executar a suíte completa de testes unitários, de repository e de integração contra MySQL e corrigir falhas relacionadas à mudança.
- [ ] 9.2 Executar `dotnet build` e validar migrations e inicialização da aplicação sem conexão automática durante o registro do contexto.
- [ ] 9.3 Exercitar no Swagger os fluxos de cadastro, relacionamento, contrato, nota fiscal e pagamento e registrar os resultados da validação.
