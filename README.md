
Library Management System API
A Library Management System API built with ASP.NET Core to manage books, patrons, and borrowing records.

Requirements
.NET 6 SDK or later
SQL Server
Setup Instructions
1. Clone the Repository
bash
 
git clone https://github.com/BilalAljaddoua/library-management-system.git
cd library-management-system
2. Restore the Database
Restore Library.bak into your SQL Server instance.
This will create the required database schema and data for the library management system.
3. Configure the Connection String
Open the class clsDataSettings in the clsLibraryData project.

Update the connection string to match your SQL Server settings (e.g., UserID, Password, and server details).

Example:

csharp
 
public static string ConnectionString = "Server=your_server;Database=LibraryDB;User Id=your_username;Password=your_password;";
4. Run the Application
After completing the setup, you can run the application:

bash 
dotnet run
The API will be available at http://localhost:5000 by default. You can test the API endpoints using Swagger (if enabled) or any REST client.
