# CloudPins 📌☁
![Banner CloudPins](./assets/preview.gif)
CloudPins é uma plataforma de curadoria visual que permite organizar, salvar e descobrir novas imagens por meio de coleções.

A plataforma foi projetada com foco em escalabilidade, separação de responsabilidades e armazenamento eficiente de mídia, utilizando arquitetura limpa, domínio bem definido e um modelo de leitura otimizado para feeds.

# Teste o projeto rapidamente utilizando Docker 🐋
/cloudpins/docker-compose.yml
```
docker compose up --build -d
```

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
    <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dot-net/dot-net-plain-wordmark.svg" height="40" alt="dot-net logo"  />
    <img width="12" />
      <img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/amazonwebservices/amazonwebservices-plain-wordmark.svg" height="40"/>  
    <img width="12" />
</div>

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

## Testabilidade
O projeto foi estruturado para facilitar a criação de testes automatizados.

# 💫 Próximas Features
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
