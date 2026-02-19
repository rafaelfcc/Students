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
- **Domain**: Entidades, contratos e regras centrais

> Importante: **Controllers não acessam o Repository diretamente**. Toda interação passa pela camada de Services.

---

## Rodando com Docker (recomendado)

### Pré-requisitos
- Docker + Docker Compose

### Subir o ambiente

Na raiz do projeto (onde está o `docker-compose.yml`):

```bash
docker compose up -d --build
