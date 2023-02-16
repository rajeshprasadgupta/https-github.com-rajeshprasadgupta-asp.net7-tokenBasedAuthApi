This repo:
* Has ASP.NET Core Minimal API to showcase JWT Token based Authentication to API endpoints
* Uses .NET 7 based ASP.NET Core Minimal API
* Uses ASP.NET Core Identity for managing users and roles
* Uses EntityFramwork Core to communicate to SQLite database
* Has Endpoints that generates JWT Bearer token to protect the API endpoints access
* All API endpoints requests need a valid JWT Token in the request header
* Has Endpoints for user identity CRUD operations

Run the commands (mentiond in Commands Section) to create SQLite Database with Identity tables and following two test users
1. User "user" having password "P@ssword1" with "user" role
2. User "admin"having password "P@ssword1" with "admin" role

Commands:
1. dotnet ef migrations add "addIdentityTablesAndTestData" --context AppDbContext --project TokenBasedAuthApi
2. dotnet ef database update --context AppDbContext --project TokenBasedAuthApi


How to use?

Use "/api/auth/login" to login and generate token

Supply the generated token in the Authorize Section in the Swagger page. Token should be in the format: Bearer <token>

When you invoke rest other endpoints, swagger will send the token in the request header  
