# CloudPins 📌☁
![Banner CloudPins](./assets/preview.gif)
CloudPins é uma plataforma de curadoria visual que permite organizar, salvar e descobrir novas imagens por meio de coleções.

A plataforma foi projetada com foco em escalabilidade, separação de responsabilidades e armazenamento eficiente de mídia, utilizando arquitetura limpa, domínio bem definido e um modelo de leitura otimizado para feeds.

# Primeiro Acesso 🔒
Email:
```
admin@gmail.com
```
Senha:
```
123
```
Você também pode optar por criar uma nova conta! 😉

# Teste o projeto rapidamente utilizando Docker 🐋
/cloudpins/docker-compose.yml
```
docker compose up --build -d
```

## Tecnologias utilizadas
<div align="center">
    <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/docker/docker-plain-wordmark.svg" height="40" alt="docker logo"  />
     <img width="12" />
    <img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/typescript/typescript-original.svg" height="40"/>
    <img width="12" />
    <img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/react/react-original.svg" height="40"/>
    <img width="12" />
    <img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/csharp/csharp-original.svg" height="40"/>
    <img width="12" />
    <img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/elasticsearch/elasticsearch-original.svg" height="40"/>
    <img width="12" />
    <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dot-net/dot-net-plain-wordmark.svg" height="40" alt="dot-net logo"  />
    <img width="12" />
      <img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/amazonwebservices/amazonwebservices-plain-wordmark.svg" height="40"/>  
    <img width="12" />
</div>


# 📚 Organização Do projeto
- **CloudPins.Domain:** contém as entidades, regras de negócio e agregados
- **CloudPins.Application:** orquestra os casos de uso da aplicação.
- **CloudPins.Infrastructure:** implementações como (PostgreSQL, AWS S3)
- **CloudPins.Api:** exposição dos endpoints
- **CloudPins.App:** SPA React + Typescript
- **CloudPins.Tests:** Testes unitários


## 👑 MVP do CloudPins
O sistema permite que um usuário:
- Crie uma conta
- Crie boards
- Faça upload de imagens
- Associe pins a coleções
- Veja um feed público de pins
- Dê like em pins

# 📲 Feed
- Pins de Boards públicas
- Ordem de Relevância
- Scroll infinito


# 🏠 Decisões de Arquiteturas
## DDD
O projeto foi estruturado seguindo os princípios de Domain Driven Design, com o objetivo de manter o domínio da aplicação isolado das preocupações de infraestrutura e interface.

O Domínio da aplicação é composto principalmente pelos agregados:
- **User:** representa o usuário da plataforma
- **Board:** coleções de pins por usuário
- **Pin:** imagem publicada que pertence a uma board

## CQRS
O sistema utiliza CQRS para separar operações de leitura e escrita.

Essa decisão foi tomada considerando o comportamento esperado da aplicação:
**Operações de leitura ocorrem com mais frequência do que operações de escrita.**

Exemplos de leitura incluem:
- Visualização de feed público
- Exploração de boards
- Navegação entre pins

Operações de escrita incluem:
- Upload de imagens
- Criação de boards
- Associação de pins a coleções

# Elasticsearch
O CloudPins utiliza Elasticsearch para realizar buscas full-text nos pins.

O servico e executado pelo Docker Compose:

```text
Elasticsearch: http://localhost:9200
Indice: cloudpins_documents
```

Cada documento indexado representa um pin e contem informacoes como:

- titulo
- descricao
- tags
- URLs da imagem e thumbnail
- quantidade de likes
- data de criacao

A busca pesquisa principalmente nos campos `title`, `description` e `tags`, dando maior relevancia ao titulo.

O endpoint de busca da API e:

```text
GET http://localhost:5023/search/{termo}
```

Exemplo:

```text
http://localhost:5023/search/car?page=1&pageSize=20
```

A resposta possui paginacao:

```json
{
  "items": [],
  "page": 1,
  "pageSize": 20,
  "total": 0,
  "totalPages": 0
}
```

O Elasticsearch e sincronizado quando pins sao criados, atualizados ou excluidos. A API tambem possui fallback para o PostgreSQL caso o Elasticsearch esteja indisponivel.

Comandos uteis para verificar o indice:

```powershell
curl.exe http://localhost:9200
curl.exe http://localhost:9200/_cat/indices?v
curl.exe http://localhost:9200/cloudpins_documents/_count
curl.exe http://localhost:9200/cloudpins_documents/_mapping
```

Para reconstruir o indice durante o desenvolvimento:

```powershell
curl.exe -X DELETE http://localhost:9200/cloudpins_documents
docker compose up -d --build api
```

## Testabilidade
O projeto foi estruturado para facilitar a criação de testes automatizados.
```
dotnet test CloudPins.Domain.Tests\CloudPins.Domain.Tests.csproj
```

# 💫 Próximas Features
- Compartilhar pin
- Cache de feed com Redis
- Content Moderation

## 🍇 Comandos Migrations
### ➕ Criar um nova migration
```
dotnet ef migrations add NomeDaMigrationNova --project CloudPins.Infrastructure --startup-project CloudPins.Api
```
### 🔄 Atualizar Banco
```
dotnet ef database update --project CloudPins.Infrastructure --startup-project CloudPins.Api
```

## 🎲 Tabelas do Banco

👦 Users 
| Id | Name | Email | ProfileUrl |
|----|------|-------|------------|
| Guid | string | string | string |

 📁 Boards
| Id | OwnerId | Name | Description | IsPublic |
|----|---------|------|-------------|----------|
| Guid | Guid | string | string | bool |

📌 Pins
| Id | OwnerId | BoardId | ImageUrl | ThumbnailUrl | Title | Description | Tags | LikesCount |
|----|---------|---------|----------|--------------|-------|-------------|------|------------|
| Guid | Guid | Guid | string | string | string | string | string[] | int |

🏷 Tag
| Id | Name |
|----|------|
| string | string |

✒ PinTag
| PinId | TagId |
|-------|-------|
| Guid | Guid |

❤ Likes
| UserId | PinId |
|--------|-------|
| Guid | Guid |
