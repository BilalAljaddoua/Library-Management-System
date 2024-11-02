# Library Management System API

**Library Management System API** is an API built using **ASP.NET Core** for managing books, members, and loan records.

## Requirements
- **.NET 6 SDK** or later
- **SQL Server**

## Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/BilalAljaddoua/library-management-system.git
cd library-management-system
```

### 2. Restore the Database
- **Restore the `Library.bak` file** to your SQL Server.
- This will provide the necessary structure and data for the system to operate.

### 3. Set Up Database Connection
- Open the **`clsDataSettings` class** in the `clsLibraryData` project.
- Update the Connection String to match your SQL Server settings, including `UserID`, `Password`, and server details.

#### Example:
```csharp
public static string ConnectionString = "Server=your_server;Database=LibraryDB;User Id=your_username;Password=your_password;";
```

### 4. Run the Application
After completing the setup, you can run the application using the following command:

```bash
dotnet run
```

- The **Swagger** interface will be available by default at `http://localhost:5027/swagger/index.html`.
- You can test the API endpoints using Swagger (if enabled) or any REST client.

---
