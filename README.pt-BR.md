# ToDo API

[![English](https://img.shields.io/badge/lang-en-red.svg)](README.md)
[![Português](https://img.shields.io/badge/lang-pt--br-green.svg)](README.pt-BR.md)

API RESTful para gerenciamento de tarefas (To-Do) desenvolvida com ASP.NET Core e .NET 9.0.  
A aplicação possui autenticação JWT Bearer, integração com SQL Server e mecanismos de confirmação de identidade via e-mail utilizando MailKit.

O projeto foi construído seguindo boas práticas de desenvolvimento de software, incluindo Clean Code e Domain-Driven Design (DDD).

---

## Tecnologias utilizadas

- ASP.NET Core (.NET 9.0)
- SQL Server
- JWT Bearer Authentication
- MailKit
- OpenAPI / Swagger
- Docker

---

## Funcionalidades

- Registro de usuários
- Login com autenticação JWT
- Confirmação de e-mail
- Recuperação de senha
- Reset de senha via token
- Integração com banco de dados SQL Server
- Documentação OpenAPI (Swagger)

---

# Endpoints

## Autenticação

### Registrar usuário

`POST /api/authentication/register`

#### Request Body

```json
{
  "userName": "douglas",
  "email": "douglas@email.com",
  "password": "123456",
  "phoneNumber": "69999999999",
  "firstName": "Douglas",
  "lastName": "Souza",
  "bio": "Software developer"
}
```

---

### Login

`POST /api/authentication/login`

#### Request Body

```json
{
  "email": "douglas@email.com",
  "password": "123456"
}
```

---

### Enviar token de confirmação de e-mail

`POST /api/authentication/email-confirmation/send`

#### Request Body

```json
{
  "email": "douglas@email.com",
  "name": "Douglas"
}
```

---

### Confirmar e-mail

`POST /api/authentication/email-confirmation/verify`

#### Request Body

```json
{
  "email": "douglas@email.com",
  "token": "123456"
}
```

---

### Enviar token de recuperação de senha

`POST /api/authentication/password-reset/send`

#### Request Body

```json
{
  "email": "douglas@email.com",
  "name": "Douglas"
}
```

---

### Redefinir senha

`POST /api/authentication/password-reset/verify`

#### Request Body

```json
{
  "email": "douglas@email.com",
  "token": "123456",
  "password": "newPassword123"
}
```

---

# Autenticação

A API utiliza autenticação do tipo JWT Bearer.

Exemplo de header:

```http
Authorization: Bearer {token}
```

---

# Executando o projeto

## Docker

```bash
docker compose up -d
```

---

# Documentação Swagger

Após executar a aplicação, a documentação OpenAPI estará disponível em:

```txt
http://localhost:{porta}/swagger
```

---

# Arquitetura

O projeto segue conceitos de:

- Clean Code
- Domain-Driven Design (DDD)
- Separação por camadas
- Boas práticas para APIs RESTful

---

# Licença

Este projeto está disponível para fins de estudo e portfólio.