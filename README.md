
#  BeSafe 

[![Oracle](https://img.shields.io/badge/Oracle-F80000?style=for-the-badge&logo=oracle&logoColor=white)](https://www.oracle.com/database/)
[![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)](https://www.rabbitmq.com/)

## 📝 Descrição do Projeto

O Sistema de Alertas de Desastres é uma solução completa para monitoramento e notificação de situações de emergência como enchentes, deslizamentos, desabamentos e outros eventos climáticos extremos. O sistema é composto por duas aplicações:

1.  **API Principal**  - Aplicação .NET que gerencia cadastros de usuários e áreas de risco, permitindo o registro e gerenciamento de alertas.

2.  **Serviço de Notificação**  - Aplicação que processa alertas e notifica usuários localizados em áreas de risco por email.


O sistema utiliza mensageria para comunicação entre os componentes, garantindo escalabilidade e confiabilidade nas situações críticas onde cada segundo conta! 🕒

## 🛠️ Tecnologias Utilizadas

### API Principal

-   **.NET 8.0**  - Framework base da aplicação
-   **ASP.NET Core Web API**  - Para criação dos endpoints REST
-   **Entity Framework Core 8.0.3**  - ORM para acesso ao banco de dados
-   **Oracle.EntityFrameworkCore 8.21.121**  - Provider para Oracle Database
-   **RabbitMQ.Client 6.8.1**  - Cliente para comunicação com o broker de mensagens
-   **Swagger/OpenAPI**  - Documentação interativa da API

### Serviço de Notificação

-   **.NET 8.0**  - Framework base da aplicação
-   **Entity Framework Core 8.0.3**  - ORM para acesso ao banco de dados
-   **Oracle.EntityFrameworkCore 8.21.121**  - Provider para Oracle Database
-   **RabbitMQ.Client 6.8.1**  - Cliente para consumo de mensagens
-   **Azure.Communication.Email 1.0.1**  - SDK para envio de emails via Azure
-   **Microsoft.Extensions.Hosting**  - Para execução como serviço em background

### Infraestrutura

-   **Oracle Database**  - Armazenamento de dados
-   **RabbitMQ**  - Broker de mensagens para comunicação assíncrona
-   **Azure Communication Services**  - Serviço de envio de emails em larga escala

## 🚀 Como Executar o Projeto

### Pré-requisitos

-   [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)  ou superior
-   [Oracle Database](https://www.oracle.com/database/technologies/oracle-database-software-downloads.html)  (18c ou superior)
-   [RabbitMQ](https://www.rabbitmq.com/download.html)  instalado e rodando
-   Conta no  [Azure Communication Services](https://azure.microsoft.com/pt-br/services/communication-services/)  com domínio de email verificado

### Configuração do Banco de Dados

1.  Execute os scripts SQL fornecidos para criar as tabelas e inserir dados iniciais:
### Modelagem Banco
![modelagem](https://i.ibb.co/SXTq6S6y/Screenshot-3.png)

 ```
 CREATE TABLE GS_USUARIO(
    ID INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    NOME VARCHAR2(255) NOT NULL,
    CPF VARCHAR2(11),
    DATA_NASCIMENTO DATE NOT NULL
);

CREATE TABLE GS_CONTATO(
    ID INTEGER PRIMARY KEY,
    EMAIL VARCHAR2(120) NOT NULL,
    TELEFONE VARCHAR2(11) NOT NULL,
    CONSTRAINT fk_contato_usuario FOREIGN KEY(ID)
        REFERENCES GS_USUARIO(ID)
);

CREATE TABLE GS_ENDERECO_USUARIO(
    ID INTEGER PRIMARY KEY,
    LOGRADOURO VARCHAR2(120) NOT NULL,
    BAIRRO VARCHAR2(80) NOT NULL,
    CIDADE VARCHAR2(80) NOT NULL,
    ESTADO VARCHAR2(50) NOT NULL,
    CONSTRAINT FK_ENDERECO_USUARIO_USUARIO FOREIGN KEY(ID)
        REFERENCES GS_USUARIO(ID)
);

CREATE TABLE GS_AREA_RISCO(
    ID INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    MENSAGEM CLOB NOT NULL,
    DATA_ENVIO DATE,
    TIPO_ALERTA NUMBER(2)
);

CREATE TABLE GS_ALERTA(
    ID INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    AREA_RISCO_ID INTEGER NOT NULL,
    MENSAGEM CLOB NOT NULL,
    DATA_ENVIO DATE NOT NULL,
    TIPO_ALERTA VARCHAR(40) NOT NULL,
    CONSTRAINT fk_area_risco_alerta FOREIGN KEY(AREA_RISCO_ID)
        REFERENCES GS_AREA_RISCO(ID)
);
 ```


### API Principal (DisasterAlertSystem)

1.  Clone o repositório


    git clone https://github.com/wedsan/disaster-alert-system.git
    cd .net-fiap-final-gs/BeSafe

2.  Configure a string de conexão no arquivo  `appsettings.json`


    "ConnectionStrings": {
      "DefaultConnection": "User Id=seu_usuario;Password=sua_senha;Data Source=seu_oracle_datasource;"
    }


3.  Configure as credenciais do RabbitMQ


    "RabbitMQ": {
      "HostName": "localhost",
      "UserName": "guest",
      "Password": "guest"
    }


4.  Execute a aplicação


    dotnet run


5.  Acesse a documentação Swagger:  `https://localhost:5001/swagger`


### Serviço de Notificação (AlertNotificationService)

1.  Navegue para o diretório do serviço


    cd ../AlertNotificationService


2.  Configure o arquivo  `appsettings.json`  com as informações do banco de dados, RabbitMQ e Azure Communication Services


    {
      "ConnectionStrings": {
        "DefaultConnection": "User Id=seu_usuario;Password=sua_senha;Data Source=seu_oracle_datasource;"
      },
      "RabbitMQ": {
        "HostName": "localhost",
        "UserName": "guest",
        "Password": "guest"
      },
      "Azure": {
        "CommunicationServices": {
          "Version": "1.0.1",
          "Endpoint": "https://seu-servico.communication.azure.com",
          "ConnectionString": "endpoint=https://seu-servico.communication.azure.com/;accesskey=sua-chave-acesso",
          "SenderEmail": "alertas@seudominio.com"
        }
      }
    }

3.  Execute o serviço

    dotnet run

## 📊 Documentação dos Endpoints

### API Principal

#### 👤 Usuários

-   **GET**  `/api/User`  - Lista todos os usuários cadastrados
-   **GET**  `/api/User/{id}`  - Obtém um usuário específico com seus contatos e endereços
-   **POST**  `/api/User`  - Cadastra um novo usuário
-   **PUT**  `/api/User/{id}`  - Atualiza dados de um usuário
-   **DELETE**  `/api/User/{id}`  - Remove um usuário

#### 🗺️ Áreas de Risco

-   **GET**  `/api/RiskArea`  - Lista todas as áreas de risco cadastradas
-   **GET**  `/api/RiskArea/{id}`  - Obtém uma área de risco específica com seus alertas
-   **POST**  `/api/RiskArea`  - Cadastra uma nova área de risco
-   **PUT**  `/api/RiskArea/{id}`  - Atualiza uma área de risco
-   **DELETE**  `/api/RiskArea/{id}`  - Remove uma área de risco

#### ⚠️ Alertas

-   **GET**  `/api/Alert`  - Lista todos os alertas emitidos
-   **GET**  `/api/Alert/{id}`  - Obtém detalhes de um alerta específico
-   **GET**  `/api/Alert/riskarea/{riskAreaId}`  - Lista alertas por área de risco
-   **POST**  `/api/Alert`  - Emite um novo alerta (também envia para o RabbitMQ)
-   **PUT**  `/api/Alert/{id}`  - Atualiza um alerta existente
-   **DELETE**  `/api/Alert/{id}`  - Remove um alerta

## 📨 Fluxo de Envio de Alertas

1.  Um operador cadastra um alerta através da API Principal
2.  O alerta é salvo no banco de dados
3.  Uma mensagem é publicada no RabbitMQ
4.  O Serviço de Notificação consome a mensagem
5.  O serviço identifica usuários na área de risco
6.  Emails personalizados são enviados para esses usuários via Azure Communication Services

## 🔍 Monitoramento

O sistema registra logs detalhados de todas as operações, facilitando o diagnóstico de problemas e o acompanhamento de atividades críticas durante emergências.

## 🔐 Segurança

-   Todas as comunicações são protegidas usando HTTPS
-   As credenciais são armazenadas de forma segura em arquivos de configuração
-   O acesso à API pode ser protegido usando autenticação JWT (implementação opcional)