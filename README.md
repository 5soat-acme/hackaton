# Introdução :spiral_notepad:

Este projeto é para contemplar a atividade proposta de Hackaton do curso de Pós Graduação em Software Architecture. O projeto deve contemplar os requistos neste [documento](docs/requisitos_hackaton/Hackathon_SOAT.pdf). O projeto tem o intuito é aplicar os conceitos aprendidos durante o curso.

# Tecnologias utilizadas :computer:
- .NET 8.0
  - ASP.NET Web API
  - Entity Framework Core
- PostgreSQL
- Docker
- Kubernetes
- AWS

# Arquitetura :triangular_ruler:
## Patterns utilizados
- Monolito
- Clean Architecture
- Domain Validations
- Repository Pattern
- Unit Of Work Pattern
- Testes unitários

## Introdução :mag:

Na primeira fase do projeto, foi desenvolvido um monolito modular para fazer uma separação clara dos contextos delimitados mapeados na modelagem estratégica. Separamos a implementação em 3 pastas principais:
- **Presentation:** É a camada que expõe os serviços da aplicação. É responsável por receber as requisições HTTP, fazer a validação dos dados de entrada, mapear os dados de entrada para os objetos de domínio, chamar os serviços de aplicação e retornar os dados de saída.
- **Services:** É onde estão implementados os serviços de aplicação. Dentro desta pasta dividimos em subpastas que representam os contextos delimitados. Cada subpasta contém as camadas do serviço, como **Application, Domain, Infra**, entre outras.
- **Shared:** É aqui que compartilhamos o que é comum entre os diferentes módulos, inclusive os objetos de domíno e os serviços de infraestrutura que podem ser utilizados por mais de um contexto delimitado.



Foi desenvolvido um monolito modular fazer uma separação clara dos contextos delimitados. </br>
O desenvolvimento foi pensado para ficar de uma forma clara, separando cada contexto em um projeto. Dessa maneira, fica clara a identificação dos contextos e facilita no desenvolvimento, podendo separar o desenvolvimento dos contextos entre os times, sem que haja conflitos. Separamos a implementação em 3 pastas principais:
- `Presenter:` É a camada que expõe os serviços da aplicação. É responsável por receber as requisições HTTP, validar os dados de entrada, injetar as dependências necessárias para a camada de aplicação e retornar para o solicitante os dados de saída.
- `Services:` São os serviços da aplicação separados por contextos delimitados. Cada contexto delimitado possui o seu core (camadas de aplicação e domínio) e infra.
- `Commons:` É aqui que compartilhamos o que é comum entre os diferentes módulos, inclusive os objetos de domíno e os serviços de infraestrutura que podem ser utilizados por mais de um contexto delimitado.


## Estrutura do projeto :hammer:
Todos os 3 microsserviços seguem o mesmo padrão. Para a explicação, apresentamos como base o microsserviço de **Pagamento**.

![img_estrutura_projeto.png](docs/img/img_estrutura_projeto.png) </br>

## Clean Architecture :o:
![clean_architecture.jpg](docs/img/clean_architecture.jpg) </br>

Cada serviço possui o seu core e sua camada de infraestrutura.

- `src`
    - `Services.*.Domain:` São os projetos referente a camada **Enterprise Business Rules** da Clean Architecture. São nesses projeto onde está o domínio da aplicação, com as regras de negócio e sem referência a nenhuma bliblioteca ou framework. Esta camada expõe interfaces que serão implementadas nas camadas externas, seguindo a regra de Inversão de Dependência.

    - `Services.*.Infra:` Nesses projetos fazemos do padrão Repository um Gateway da camada **Interface Adapters** da Clean Architecture, onde esses Repositories irão chamar um DBContext do ORM(Entity Framework). Nesse cenário, o ORM passa a ser a camada **Frameworks & Drivers** da Clean Architecture. Toda regra de acesso a dados fica na camada mais externa. Quando necessário mudar a forma de acesso a dados, basta implementar uma nova classe com base na interface e chamar uma nova implementação da camada **Frameworks & Drivers**.

    - `Services.*.Application:` São os projetos referente a camada **Application Business Rules** da Clean Architecture. São nesses projetos onde são implementados cada UseCase do sistema. Os UseCases recebem via injeção de depêndencia os Gateways(Repositories) e os utilizam de acordo com a regra de negócio necessária.

    - `Presenter:` São os Controllers da camada **Interface Adapters** da Clean Architecture. Esses Controllers são responsáveis por chamarem os UseCases passando via injeção de dependência toda dependência necessária, como por exemplo, os Gateways(Repositories). A instanciação desses Gateways são feitas utilizando a Injeção de Dependência já existente no framework do .NET. Nesse cenário a injeção de dependência do framework passa a ser a  camada **Frameworks & Drivers** da Clean Architecture.

- `test`
    - `Unit:` Projetos com a implementação de testes unitários.

## Diagramas da Arquitetura :bar_chart:

### AWS
![diagrama_aws.png](docs/diagramas/diagrama_aws.png)

### K8S
![diagrama_k8s.png](docs/diagramas/diagrama_k8s.png)


# Como executar - AWS :rocket:
A seguir estão as instruções para executar o projeto

## Pré-requisitos :clipboard:
### 1. Configuração de Secrets no GitHub

Para garantir o funcionamento correto dos workflows, é necessário configurar as seguintes secrets no GitHub, com base nas credenciais das contas AWS Academy, Docker Hub e SonarCloud:
- AWS_ACCESS_KEY_ID
- AWS_SECRET_ACCESS_KEY
- AWS_SESSION_TOKEN
- AWS_REGION
- DOCKERHUB_TOKEN
- SONAR_TOKEN
- DATABASE_PASSWORD

### 2. Criação da Infraestrutura de Banco de Dados
Execute a GitHub Action para criar a infraestrutura de banco de dados. Para isso, ajuste o arquivo [variables.tf](terraform/database/variables.tf) com as informações da AWS, crie um Pull Request com as alterações e faça o merge. Esse processo disparará o workflow responsável pela criação da infraestrutura de banco de dados.

### 3. Criação da Infraestrutura EKS e Configuração do appsettings.json
Após a criação da infraestrutura do banco de dados, o próximo passo é criar a infraestrutura do EKS e configurar a aplicação .NET.

- Atualize o arquivo [variables.tf](terraform/eks/variables.tf): Insira as informações da AWS necessárias.
- Atualize o arquivo [secret.yaml](terraform/eks/kubernetes/secret.yaml): Insira o conteúdo do arquivo `appsettings.json` da API .NET, substituindo o valor `{host}` pelo endereço do banco de dados Aurora PostgreSQL criado no passo anterior.

Exemplo de appsettings.json:
``` bash
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host={host};Port=5432;Database=hackaton;Username=postgres;Password=acmeacme"
  },
  "IdentidadeSettings": {
    "Secret": "E076B751-88AC-4344-A931-88ABFD665916",
    "ExpirationHours": 2,
    "Issuer": "Hackaton",
    "ValidIn": "Hackaton-Api"
  }
}
```

### 4. Deploy da API
Com a infraestrutura pronta, a API estará disponível utilizando a última imagem publicada no DockerHub. Para realizar o deploy de uma nova versão da API, siga os passos:
1. Faça as alterações necessárias no código da API.
2. Crie um Pull Request e faça o merge.

O workflow será iniciado automaticamente, gerando uma nova imagem Docker, executando os testes unitários e publicando a imagem no DockerHub. Um `rollout restart` será aplicado automaticamente ao deployment no cluster EKS.


Para acessar o projeto no SonarCloud, [clique aqui](https://sonarcloud.io/summary/overall?id=5soat-acme_hackaton)


## Como utilizar :bulb:

Após toda a infraestrutura criada:
- Configurar **kubeconfig** com o comando
```
aws eks update-kubeconfig --region us-east-1 --name hackaton
```
- Com o comando abaixo buscar o link do LoadBalancer criado pelo Ingress NGINX Controller.
```
kubectl get service -n nginx-ingress
```

A URL de acesso será o conteúdo da coluna **EXTERNAL-IP** do serviço de tipo LoadBalancer.
A documentação estará disponível em: 
 - EXTERNAL-IP/swagger

# Utilização dos Endpoints :arrow_forward:
Os acessos aos endpoints são controlados por **[Json Web Token (JWT)](https://jwt.io/)**
Exemplos:
- Ao criar uma agenda, ela será automaticamente vinculada ao médico que está autenticado.
- Ao criar um agendamento, ele será associado ao paciente que está autenticado.
Essas informações são extraídas do token JWT utilizado na autenticação.

### Paciente
1. Pode-se criar o cadastro de um paciente em: `[POST] api/pacientes`

### Médico
1. Pode-se criar o cadastro de um médico em: `[POST] api/medicos`
2. Pode-se consultar os médicos disponíveis em: `[GET] api/medicos`

### Login
1. Pode-se efetuar o login em: `[POST] api/usuarios/acessar`

### Agenda
1. Pode-se criar uma agenda(data/hora) para o médico em:: `[POST] api/agendas`
2. Pode-se remover uma agenda do médico em: `[DELETE] api/agendas/{id}`
3. Pode-se consultar as agendas disponíveis de um médico em: `[GET] api/agendas/medico/{medicoId}`

### Agendamento
1. Pode-se efetuar um agendamento para o paciente em: `[POST] api/agendamentos`

<br><br>

## **Pronto! Agora você já pode utilizar a API** :smile: