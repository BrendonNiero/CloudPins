# CloudPins 📌☁
![Banner CloudPins](./assets/preview.gif)
CloudPins é uma plataforma de curadoria visual que permite organizar, salvar e descobrir novas imagens por meio de coleções.

A plataforma foi projetada com foco em escalabilidade, separação de responsabilidades e armazenamento eficiente de mídia, utilizando arquitetura limpa, domínio bem definido e um modelo de leitura otimizado para feeds.

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
- Ordem de Relevancia
- Infinit Scroll


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
