# Students API (Backend)

Backend do projeto **Students**, desenvolvido em **ASP.NET Core** com arquitetura em camadas, autenticação **JWT**, documentação via **Swagger** e observabilidade com **Serilog**. O projeto está **dockerizado** para rodar localmente com um único comando.

---

## Stack / Tecnologias

- **.NET 8** (ASP.NET Core Web API)
- **Entity Framework Core**
- **JWT Authentication**
- **Swagger** (com suporte a bearer token)
- **Serilog** (Console + Rolling File)
- **Docker / Docker Compose**

---

## Arquitetura

O projeto segue uma arquitetura em camadas:

- **API**: Controllers, validações de entrada e DTOs
- **Services / Application**: Regras de negócio e orquestração
- **Repositories / Infra**: Persistência de dados (EF Core) e acesso a banco
    - **Projeto de Testes xUnit**: Existe um projeto de testes unitários para a camada de repositórios.   
- **Data**: Instruçòes de acesso a base de dados
- **Domain**: Entidades e contratos de repositório

---

## Rodando com Docker (recomendado)

### Pré-requisitos
- Docker + Docker Compose

### Subir o ambiente

Tendo uma instalação do Docker Desktop, abra alguma ferrametna de prompt de comando na raiz do projeto (onde está o `docker-compose.yml`) e execute:

```bash
docker compose up -d --build
```

Caso queira testar os endpoints isoladamente pelo Swagger, na parte de autenticação, coloque apenas a string do token gerada dinamicamente, sem escrever 'Bearer' no início
