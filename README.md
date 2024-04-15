# NZWalks ASP.NET Web API
This project is a RESTful API built using ASP.NET Web API framework, providing endpoints to access information about walks and trails in New Zealand.

## Technologies Used
- **ASP.NET Web API**: The primary framework for building HTTP services that provide endpoints for interacting with walk and trail data.
- **Microsoft SQL Server (MSSQL)**: A relational database management system used to store and manage information about walks and trails.
- **Entity Framework Core (EF Core)**: An object-relational mapper used for data access and manipulation within the application. EF Core enables .NET developers to work with databases using .NET objects.
- **JWT (JSON Web Tokens)**: JWT is used for authentication and authorization purposes. It allows secure transmission of claims between parties using a compact and self-contained JSON object.
- **ASP.NET Identity**: ASP.NET Identity is used for managing user authentication and authorization. It provides features such as user registration, login, and role-based access control.
- **Swagger**: Swagger is used to document the Web API endpoints. It provides a user-friendly interface for developers to explore and test the API's functionality.

## Endpoints
The API provides the following endpoints:
### Auth
- /api/Auth/Register: Allows users to create a new account by providing their registration information.
- /api/Auth/Login: Allows registered users to log in and obtain a JWT token for accessing protected endpoints.
### Images
- /api/Images/Upload: Allows users to upload image.
### Regions
- /api/Regions: Create and Read operation for regions.
- /api/Regions/{id}: Read, Update, Delete operation for region by id.
### Walks
- POST: /api/Walks: Create new walks.
- GET: /api/Walks: Read walks and also you can filter, sort and paging.
- /api/Walks/{id}: Read, Update, Delete operation for walks by id.

## Screenshots
#### Login
![image](https://github.com/Porcelainz/NZWalks/assets/63963971/1aef35a8-cd55-4bdc-8090-7885dc7bd7b2)
![image](https://github.com/Porcelainz/NZWalks/assets/63963971/5fb8066f-2763-4f7f-815d-3780a3a6b7cc)
#### Regions
![image](https://github.com/Porcelainz/NZWalks/assets/63963971/b9af428e-47bf-499d-a015-436bd2905c32)
![image](https://github.com/Porcelainz/NZWalks/assets/63963971/83e05b6d-5081-412c-8982-15af05eaae94)
#### Walks
![image](https://github.com/Porcelainz/NZWalks/assets/63963971/2d3b6ee9-568c-4e22-b159-54d1d5690b3f)
![image](https://github.com/Porcelainz/NZWalks/assets/63963971/42c1edb7-b36c-451e-ac8c-0f315704304e)
