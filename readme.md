# CloudPins 📌☁
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

## 🍇 Comandos Migrations
### ➕ Criar um nova migration
```
dotnet ef migrations add NomeDaMigrationNova --project CloudPins.Infrastructure --startup-project CloudPins.Api
```
### 🔄 Atualizar Banco
```
dotnet ef database update --project CloudPins.Infrastructure --startup-project CloudPins.Api
```

## 🚩 Bounded Context

### Entidades Principais
👦 Users
- Id
- Name
- Email
- ProfileUrl

 📁 Boards
- Id
- OwnerdId
- Name
- Description
- IsPublic

📌 Pins
- Id
- OwnerdId
- BoardId
- ImageUrl
- ThumbnailUrl
- Title
- Description
- Tags
- CreatedAt
- LikesCount

❤ Likes
- UserId
- PinId

### ⚙ Relacionamentos 
- Users -> Boards   (1:N)
- Boards -> Pins    (1:N)
- Pins -> Likes     (1:N)

# 📲 Feed
- Pins públicos
- Ordenados por CreatedAt

