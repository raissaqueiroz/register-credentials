# register-credentials

API que Simula o Comportamento de uma Adquirente

![desafio-mini-adquirencia](https://github.com/raissaqueiroz/register-credentials/assets/48663408/a564d9b0-afed-435f-9bd7-088708244a24)

**Registro de Estabelecimento comercial**

Ao receber os dados do E.C através de uma requisição do tipo POST, deve ser feito a validação dos dados obrigatórios, e após isso, inserir os dados no banco. Antes da inserção, é necessário validar se já não existe um EC com os mesmos dados na base.

**Registro de uma transação**

Receber informações da transação e montar o payload de acordo com o que foi definido. Aplicar as regras de negocio a seguir e só então salvar na base de dados.

- asset holder sempre vai ser CPF e Nome da nossa mini adquirência. Definir em constantes.
- criar constantes com os valores a seguir Taxas mdr
    
    pix 0,05%
    
    crédito 4%
    
    débito 1%
    
- criar mapper para receber paymentScheme retornar a taxa de acordo com as constantes definidas
- Datas de Pagamento: PaymentDateBuilder ou PaymentDateMapper?
    
    5 dias uteis de prazo para SettlementObligationDate e 2 dias úteis para DueDate e SettlementDate
    
- Aplicar taxa mdr em cima do amount e definir PrePaidAmount e SettlementAmount (AmoutBuilder ou AmountMapper?)

**Listagem do estabelecimento comercial**

**Listagem de todos os estabelecimentos comerciais**

**Atualização de Estabelecimento comercial**

Deve ser possível ser feita a atualização dos dados de um E.C. através de uma requisição do tipo PUT. No cabeçalho da requisição, deve ser informado qual o id do estabelecimento que irá ser atualizado. Antes de atualizar, deve ser feito a validação para checar se existe o E.C. na base. Caso não exista, deve retornar uma mensagem/exceção.

**Inativação/ativação do estabelecimento comercial**

### CONTRATOS DOS FLUXOS

- **Processamento de Transação:**
    - amount - valor da transação
    - original asset holder (name e doc) - banco ou uma instituição financeira emitir pagamento
    - asset holder (name e doc) - pra quem vai o pagamento (adquirência)
    - PaymentScheme - bandeira e meio pagamento
    - DueDate - data de vencimento para o pagamento das taxas e tarifas
    - Domicile - pra onde vai o dinheiro
    - SettlementObligationStatus - Pending/Done/Canceled
    - ReferenceDate - Usada pra agrupar os recebiveis pra uma determinada data e concilialos através dela
    - SettlementAmount - Valor que a adquirência tem q pagar
    - SettlementDate - Data de Pagamento
    - SettlementObligationDate - Prazo final para a adquirencia pagar o logista
    - PrePaidAmount - Valor da taxa pago a adquirência
    - mdr - valor da taxa paga na transação
    - description - Descrição do pagamento
    - installments - nº de parcelas do pagamento

- **Credenciamento de Estabelecimento (EC):**
    - PaymentScheme - Bandeira(visa/master) e modalidade
    - BankAccount - Domicilio Bancário EC
    - Company Name - Nome Fantasia EC
    - Document (Type e Number) - Documento e Tipo (cpf/cnpj)
    - Status - Ativo ou não?

###Backlog de Tarefas
![image](https://github.com/raissaqueiroz/register-credentials/assets/48663408/470f911f-e242-4acf-8ae2-3e28fb1e9969)

###Links Úteis
```
💡 Plataforma gratuita para deploy [Render](https://dashboard.render.com/login)
```
```
💡 Video da **Pagarme** sobre configuração de *HealthChecks* e *Logs* na Api: [clique aqui](https://www.youtube.com/watch?v=TEtwzeyzdlc)
```
