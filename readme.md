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

## 🚩 Bounded Context

### Entidades Principais
👦 User
- Id
- Name
- Email
- ProfileUrl

 📁 Board
- Id
- OwnerdId
- Name
- Description
- IsPublic

📌 Pin
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