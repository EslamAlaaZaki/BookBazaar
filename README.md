# BookBazaar API

## Overview

**BookBazaar** is a backend-driven online marketplace for buying and selling used books. It provides a secure and scalable API for user authentication, book listing management, and purchase transactions. The backend is built with ASP.NET Core Web API, implementing JWT-based authentication to secure endpoints and ensure authorized access.

## Key Features

- **User Management:** Users can register, log in, update their profile, and retrieve user details.

- **Book Management:** Authenticated users can create, update, delete, and view books. Each book has attributes such as title, author, description, price, and condition.

- **Purchase Flow:** Users can buy listed books, and the system tracks the seller and buyer for each transaction.

- **Security:** JWT authentication secures the API, ensuring only authorized users can perform sensitive actions.

- **Logging:** Integrated Serilog for structured and persistent logging.

- **Database:** Uses Entity Framework Core to interact with a relational database (SQL Server), defining clear relationships between users and books.

## Technology Stack

- **Backend:** ASP.NET Core Web API, Entity Framework Core

- **Authentication:** JWT Bearer Token Authentication

- **Logging:** Serilog

- **Database:** Relational database with entities for Users and Books

- **API Documentation:** Swagger for interactive API testing and exploration

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started)
- SQL Server (if running without Docker)
- Git (for cloning the repo)


## Folder Structure

BookBazaar
- ├── BookBazaar.API/ **ASP.NET Core Web API project (Backend)**
- ├── BookBazaar.Application/ **Application layer (Business logic)**
- ├── BookBazaar.Domain/ **Domain models and entities**
- ├── BookBazaar.Infrastructure/ **Data access, EF Core, repository implementations**
- ├── docker-compose.yml  **Docker Compose configuration for API and DB**
- ├── BookBazaar.sln 
- ├── README.md 


## Running the Application

This project uses Docker Compose to run the backend API and SQL Server database together.

1. Make sure Docker Desktop (or Docker Engine) is installed and running on your machine.

2. From the root folder (`C:\BookBazaar`), run the following command to start the containers:
     ```bash
      docker-compose up --build
     ``` 
3. This will build and start the following services:
    - SQL Server running on port 14330
    - BookBazaar API running on port 5238 (HTTP)
4. Once started, you can access the Swagger UI to explore and test the API endpoints by navigating to:
    ```bash
       https://localhost:5238/swagger/index.html
    ``` 
![image](https://github.com/user-attachments/assets/050888fc-9591-494d-9b63-6d4160ba5997)

## API Documentation.

All secured endpoints require a JWT Bearer token in the Authorization header:
``` bash
Authorization: Bearer {token}
```
1. **AuthController**
   
| Method | Endpoint             | Description             | Request Body    | Response             | Auth Required |
|--------|----------------------|-------------------------|------------------|----------------------|----------------|
| POST   | `/api/auth/register` | Register a new user     | `CreateUserDto` | Registered user info | No             |
| POST   | `/api/auth/login`    | Login and get JWT token | `LoginUserDto`  | `{ token }`          | No             |


2. **UsersController**

| Method | Endpoint            | Description                   | Request Body    | Response          | Auth Required |
|--------|---------------------|-------------------------------|------------------|-------------------|----------------|
| PUT    | `/api/users/update` | Update current user's profile | `UpdateUserDto` | Updated user info | Yes            |
| GET    | `/api/users/{id}`   | Get user profile by ID        | None            | User profile      | Yes            |


3. **BooksController**

| Method | Endpoint              | Description                         | Request Body                  | Response            | Auth Required |
|--------|-----------------------|-------------------------------------|-------------------------------|---------------------|----------------|
| GET    | `/api/books`          | Get all books (optionally filtered) | `BookQueryDto` (query params) | List of books       | Yes            |
| GET    | `/api/books/{id}`     | Get details of a book by ID         | None                          | Book details        | Yes            |
| POST   | `/api/books`          | Create a new book                   | `CreateBookDto`               | Created book info   | Yes            |
| PUT    | `/api/books/{id}`     | Update an existing book             | `UpdateBookDto`               | Success status      | Yes            |
| DELETE | `/api/books/{id}`     | Delete a book                       | None                          | No content          | Yes            |
| POST   | `/api/books/buy/{id}` | Buy a book                          | None                          | Purchased book info | Yes            |


## Database Schema

The BookBazaar application uses a relational database with two main entities: User and Book.

**User Table**

| Column            | Type        | Description                                  |
|-------------------|-------------|----------------------------------------------|
| `Id`              | `Guid`      | Primary key, unique identifier for each user |
| `Name`            | `string`    | User's full name                             |
| `Email`           | `string`    | User's email address                         |
| `PasswordHash`    | `string`    | Hashed password for authentication           |
| `Phone`           | `string?`   | Optional phone number                        |
| `CreatedDateTime` | `DateTime`  | Timestamp when the user was created          |
| `UpdatedDateTime` | `DateTime?` | Timestamp when the user was last updated     |

**Book Table**


| Column            | Type                   | Description                                       |
|-------------------|------------------------|---------------------------------------------------|
| `Id`              | `Guid`                 | Primary key, unique identifier for each book      |
| `Title`           | `string`               | Title of the book                                 |
| `Author`          | `string`               | Author of the book                                |
| `Description`     | `string`               | Description or summary of the book                |
| `Price`           | `decimal`              | Price of the book                                 |
| `Condition`       | `BookCondition` (enum) | Condition of the book (`LikeNew`, `Good`, `Fair`) |
| `IsSold`          | `bool`                 | Indicates if the book has been sold               |
| `CreatedDateTime` | `DateTime`             | Timestamp when the book was listed                |
| `UpdatedDateTime` | `DateTime?`            | Timestamp when the book was last updated          |
| `SoldDateTime`    | `DateTime?`            | Timestamp when the book was sold (if applicable)  |
| `SellerId`        | `Guid`                 | Foreign key referencing the seller (User)         |
| `BuyerId`         | `Guid?`                | Optional foreign key referencing the buyer (User) |


