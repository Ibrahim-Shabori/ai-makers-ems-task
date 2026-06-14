# 🏢 AI Makers - Employee Management System (EMS)

A modern, full-stack Single Page Application (SPA) built to manage company departments and employees. This project demonstrates clean architecture on the backend using **.NET Web API** and a lightweight, high-performance frontend using **Vanilla JavaScript** and **Bootstrap 5**.

## ✨ Key Features

- **Single Page Architecture:** Lightning-fast navigation between Departments and Employees without page reloads using DOM manipulation.
- **Full CRUD Operations:** Create, Read, Update, and Delete capabilities for both Employees and Departments.
- **Advanced Data Table:**
  - Server-side pagination.
  - Real-time search filtering by employee name.
  - Dropdown filtering by department.
- **Modern UI/UX:** \* Sleek dark-mode interface built with Bootstrap 5 utility classes.
  - Dynamic global toast notifications for success and error states.
  - Integrated backend validation error display.
- **Relational Data Integrity:** Prevents deletion of departments that currently have active employees assigned to them.

---

## 🛠️ Technology Stack

**Frontend:**

- HTML5 / CSS3
- Vanilla JavaScript (ES6+)
- Bootstrap 5 (via CDN)
- _No build tools or NPM required._

**Backend:**

- C# / .NET Web API
- Entity Framework Core (Pagination & Filtering)
- CORS configured for local development

---

## 🚀 Getting Started

Follow these instructions to get a copy of the project running on your local machine for development and testing.

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) (or whichever version you are using)
- A modern web browser
- A code editor (like [Visual Studio Code](https://code.visualstudio.com/) or Visual Studio)
- _Optional but recommended:_ VS Code "Live Server" extension for serving the frontend.

### 1. Backend Setup (.NET API)

1. Clone the repository to your local machine:
   ```bash
   git clone [https://github.com/YOUR-USERNAME/ai-makers-ems-task.git](https://github.com/YOUR-USERNAME/ai-makers-ems-task.git)
   ```

### 2. Backend Setup (.NET Web API)

-- Navigate into the backend directory:

```bash
cd ai-makers-ems-task/backend
```

-- Restore the required .NET packages:

```bash

    dotnet restore

```

-- Update the database connection string in appsettings.json if you are using a local SQL Server.

-- Run the Entity Framework migrations to build the database schema:

```bash

 dotnet ef database update
```

-- Start the backend server:

```bash

dotnet run
```

-- The API should now be running (usually on https://localhost:7123). Keep this terminal open.

3. Frontend Setup (Vanilla JavaScript)

-- Since this is a Vanilla JS application using the fetch API, running it directly from the file system (e.g., file:///C:/...) will cause strict browser CORS errors. You must serve it through a local development server.

-- Open a new terminal window and navigate to the frontend directory:

```bash

cd ai-makers-ems-task/frontend
```

-- Recommended Method (VS Code):

     --- Open the frontend folder in Visual Studio Code.

     --- Install the Live Server extension.

     --- Right-click on index.html and select "Open with Live Server".

-- Alternative Method (Node.js):
If you have Node.js installed, you can use npx to quickly serve the folder:

```bash

 npx serve .
```

-- The application will automatically open in your default browser (typically at http://127.0.0.1:5500 or http://localhost:3000).

```

```
