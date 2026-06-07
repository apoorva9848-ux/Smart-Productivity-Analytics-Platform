# Daily Tracker - Complete Setup & Execution Guide

A full-stack application for tracking daily productivity with Angular frontend and .NET 8 backend.

## Features

- 📊 Track daily DSA, theory, and project work
- ✨ User-friendly interface with responsive design
- 🚀 Real-time updates with Angular
- 🔄 RESTful API with comprehensive documentation
- 📱 Built with Angular 16 and TypeScript
- ⚙️ .NET 8 backend with Entity Framework Core
- 📚 Swagger API documentation

---

## 📋 Table of Contents

1. [Prerequisites](#prerequisites)
2. [Quick Start (5 minutes)](#quick-start-5-minutes)
3. [Frontend Setup](#frontend-setup)
4. [Backend Setup](#backend-setup)
5. [Running the Application](#running-the-application)
6. [API Endpoints](#api-endpoints)
7. [Troubleshooting](#troubleshooting)
8. [Development Workflow](#development-workflow)

---

## Prerequisites

### Frontend Requirements
- **Node.js**: v16 or higher ([Download](https://nodejs.org))
- **npm**: v8 or higher (comes with Node.js)
- **Angular CLI**: Install globally with `npm install -g @angular/cli@16`

### Backend Requirements
- **.NET 8 SDK**: ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- **PowerShell** or **Command Prompt** (Windows)

### Verify Installation
```bash
# Check Node.js version
node --version

# Check npm version
npm --version

# Check .NET version
dotnet --version
```

---

## 🚀 Quick Start (5 minutes)

### Option 1: Using Scripts (Recommended)

#### On Windows PowerShell (as Administrator):
```powershell
# Clone or navigate to project directory
cd "c:\Users\apoor\OneDrive\Desktop\Job Switch\Project"

# Run setup script (if available)
# Or follow steps below manually
```

### Option 2: Manual Setup

#### Terminal 1: Start Frontend
```bash
cd "c:\Users\apoor\OneDrive\Desktop\Job Switch\Project"
npm install
npm start
```
✅ Frontend will be available at: **http://localhost:4200**

#### Terminal 2: Start Backend
```bash
cd "c:\Users\apoor\OneDrive\Desktop\Job Switch\Project\Backend"
dotnet run
```
✅ Backend will be available at: **http://localhost:5000**

---

## Frontend Setup

### Step 1: Install Dependencies
```bash
cd "c:\Users\apoor\OneDrive\Desktop\Job Switch\Project"
npm install
```

### Step 2: Start Development Server
```bash
npm start
```
Or use the alternative command:
```bash
ng serve
```

The application will automatically open in your browser at `http://localhost:4200/`

### Step 3: View Application
- Navigate to: **http://localhost:4200**
- The app will auto-reload when you make code changes

### Build for Production
```bash
npm run build
```
Output: `dist/calorie-tracker/`

### Run Tests
```bash
npm test
```

---

## Backend Setup

### Step 1: Navigate to Backend Directory
```bash
cd Backend
```

### Step 2: Restore Dependencies
```bash
dotnet restore
```

### Step 3: Build the Project
```bash
dotnet build
```

### Step 4: Run the Backend
```bash
dotnet run
```

The backend will start on **http://localhost:5000**

### Step 5: Access API Documentation
- Swagger UI: **http://localhost:5000/swagger/index.html**
- Swagger JSON: **http://localhost:5000/swagger/v1/swagger.json**

### Database
- **Type**: SQLite
- **File**: `Backend/DailyTracker.db` (auto-created)
- Migrations apply automatically on startup

---

## Running the Application

### Method 1: Full Stack (Recommended)

#### Open TWO terminals:

**Terminal 1 - Frontend:**
```bash
cd "c:\Users\apoor\OneDrive\Desktop\Job Switch\Project"
npm start
```

**Terminal 2 - Backend:**
```bash
cd "c:\Users\apoor\OneDrive\Desktop\Job Switch\Project\Backend"
dotnet run
```

Access the application:
- 🎨 **Frontend**: http://localhost:4200
- ⚙️ **Backend**: http://localhost:5000
- 📚 **API Docs**: http://localhost:5000/swagger/index.html

### Method 2: Backend Only (API Testing)

```bash
cd Backend
dotnet run
```

Test API endpoints using:
- **Swagger UI**: http://localhost:5000/swagger/index.html
- **Postman**, **Insomnia**, or **cURL**

### Method 3: Frontend Only (Mock Backend)

```bash
npm start
```

---

## API Endpoints

### Base URL
```
http://localhost:5000/api/daily
```

### Endpoints

#### 1. Get All Daily Logs
```http
GET /api/daily
```
**Response:**
```json
[
  {
    "id": 1,
    "date": "2026-04-25",
    "dsa": "Solved 2 LeetCode problems",
    "theory": "Studied Angular forms",
    "project": "Completed tracker feature",
    "notes": "Good progress"
  }
]
```

#### 2. Get Daily Log by ID
```http
GET /api/daily/{id}
```
**Example:** `GET /api/daily/1`

#### 3. Create Daily Log
```http
POST /api/daily
Content-Type: application/json

{
  "date": "2026-04-25",
  "dsa": "Solved LeetCode problems",
  "theory": "Studied Angular",
  "project": "Worked on tracker",
  "notes": "Good progress"
}
```
**Response:** 201 Created

#### 4. Update Daily Log
```http
PUT /api/daily
Content-Type: application/json

{
  "id": 1,
  "date": "2026-04-25",
  "dsa": "Solved 3 problems",
  "theory": "Studied RxJS",
  "project": "Updated tracker",
  "notes": "Excellent day"
}
```

#### 5. Resume Analysis
```http
POST /api/resume/analyze
Content-Type: multipart/form-data

- resumeFile: PDF or DOCX file upload
- jobDescription: Paste the target job description text
```
**Response:**
```json
{
  "matchScore": 78,
  "atsScore": 84,
  "missingSkills": ["Docker", "Azure DevOps", "Microservices"],
  "strongMatches": ["Angular", ".NET", "REST APIs"],
  "recommendations": [
    "Add a clear technical skills section near the top of your resume.",
    "Add keywords from the job description such as Docker, Azure DevOps, Microservices."
  ],
  "summary": "Your resume is a reasonable baseline, but it needs more job-specific keyword coverage and ATS-friendly structure."
}
```
**Response:** 200 OK

#### 5. Delete Daily Log
```http
DELETE /api/daily/{id}
```
**Example:** `DELETE /api/daily/1`
**Response:** 204 No Content

### Testing with cURL

#### Get All Logs
```bash
curl http://localhost:5000/api/daily
```

#### Create Log
```bash
curl -X POST http://localhost:5000/api/daily \
  -H "Content-Type: application/json" \
  -d '{
    "date": "2026-04-25",
    "dsa": "Solved problems",
    "theory": "Studied framework",
    "project": "Built feature",
    "notes": "Day summary"
  }'
```

#### Update Log
```bash
curl -X PUT http://localhost:5000/api/daily \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "date": "2026-04-25",
    "dsa": "Updated",
    "theory": "Updated",
    "project": "Updated",
    "notes": "Updated"
  }'
```

#### Delete Log
```bash
curl -X DELETE http://localhost:5000/api/daily/1
```

---

## Development Server

To start the Angular development server, run:

```bash
npm start
```

Navigate to `http://localhost:4200/` in your browser. The application will automatically reload if you change any source files.

## Building for Production

```bash
npm run build
```

The build artifacts will be stored in the `dist/productivity-tracker/` directory.

## Running Tests

```bash
npm test
```

---

## 📁 Project Structure

```
Project/
├── src/                          # Angular frontend
│   ├── app/
│   │   ├── components/
│   │   │   └── daily-tracker/
│   │   │       ├── daily-log-form/
│   │   │       └── daily-logs-table/
│   │   ├── models/
│   │   │   └── daily-log.model.ts
│   │   ├── services/
│   │   │   └── daily-tracker.service.ts
│   │   ├── app.component.ts
│   │   ├── app.module.ts
│   │   └── app-routing.module.ts
│   ├── environments/
│   ├── index.html
│   └── styles.scss
├── Backend/                      # .NET 8 backend
│   ├── Controllers/
│   │   └── DailyController.cs
│   ├── Models/
│   │   └── DailyLog.cs
│   ├── Services/
│   │   ├── IDailyLogService.cs
│   │   └── DailyLogService.cs
│   ├── Repositories/
│   │   ├── IDailyLogRepository.cs
│   │   └── DailyLogRepository.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Program.cs
│   ├── appsettings.json
│   ├── DailyTracker.API.csproj
│   └── DailyTracker.db
├── README.md
└── angular.json
```

---

## Technologies

### Frontend
- **Angular**: v16.0.0
- **TypeScript**: v5.1.6
- **RxJS**: v7.8.0
- **SCSS**: For styling

### Backend
- **.NET**: v8.0
- **Entity Framework Core**: v8.0.0
- **SQLite**: Database
- **Swagger/OpenAPI**: API documentation

---

## 🔧 Development Workflow

### 1. **Frontend Development**

```bash
# Start dev server (watch mode)
npm start

# Run tests
npm test

# Build production
npm run build

# Run specific component
ng generate component components/new-component
```

### 2. **Backend Development**

```bash
# Restore packages
dotnet restore

# Build
dotnet build

# Run with debug
dotnet run

# Create new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

### 3. **Adding a Feature**

#### Backend Steps:
1. Add model to `Models/`
2. Create repository in `Repositories/`
3. Create service in `Services/`
4. Add controller in `Controllers/`
5. Create migration: `dotnet ef migrations add FeatureName`
6. Test endpoints in Swagger

#### Frontend Steps:
1. Create component: `ng generate component components/feature`
2. Create service: `ng generate service services/feature`
3. Add routing: Update `app-routing.module.ts`
4. Create model in `models/`
5. Test in development server

---

## 🐛 Troubleshooting

### Frontend Issues

#### Issue: Port 4200 already in use
```bash
# Kill the process on port 4200 (Windows)
netstat -ano | findstr :4200
taskkill /PID <PID> /F

# Or run on different port
ng serve --port 4201
```

#### Issue: Module not found error
```bash
# Clear node_modules and reinstall
rm -r node_modules package-lock.json
npm install
```

#### Issue: CORS errors
- Ensure backend is running on http://localhost:5000
- Check CORS policy in `Backend/Program.cs`

---

### Backend Issues

#### Issue: Port 5000 already in use
```bash
# Kill the process on port 5000 (Windows)
netstat -ano | findstr :5000
taskkill /PID <PID> /F

# Or specify different port
dotnet run --urls="http://localhost:5001"
```

#### Issue: Database errors
```bash
# Reset database
rm Backend/DailyTracker.db

# Reapply migrations
dotnet ef database update
```

#### Issue: Build fails
```bash
# Clean and rebuild
dotnet clean
dotnet build

# If still failing, check .NET version
dotnet --version  # Should be 8.0 or higher
```

#### Issue: Swagger not loading
- Ensure backend is running: `http://localhost:5000/swagger/index.html`
- Check if Swagger is enabled in `Program.cs`

---

## Environment Configuration

### Frontend (`src/environments/`)

**environment.ts** (Development):
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000'
};
```

**environment.prod.ts** (Production):
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.yourdomain.com'
};
```

### Backend (`appsettings.json`)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=DailyTracker.db"
  }
}
```

---

## Testing

### Frontend Unit Tests
```bash
npm test

# Run with coverage
ng test --code-coverage

# Run specific test file
ng test --include='**/feature.spec.ts'
```

### Backend Unit Tests
```bash
dotnet test
```

### API Testing

#### Using Swagger UI
1. Navigate to: `http://localhost:5000/swagger/index.html`
2. Expand endpoint
3. Click "Try it out"
4. Enter parameters
5. Click "Execute"

#### Using Postman
1. Import collection from: `http://localhost:5000/swagger/v1/swagger.json`
2. Or create requests manually

#### Using cURL
See [API Endpoints](#api-endpoints) section above

---

## Building for Different Configurations

### Development
```bash
# Frontend
npm start

# Backend
dotnet run
```

### Production

#### Frontend
```bash
ng build --configuration production
```

#### Backend
```bash
dotnet publish -c Release
```

---

## 📚 Additional Resources

- [Angular Documentation](https://angular.io/docs)
- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Swagger OpenAPI](https://swagger.io/)
- [TypeScript Documentation](https://www.typescriptlang.org/)

---

## Next Steps

To extend the application:

1. **Create Services**: Add data management services in `src/app/services/`
2. **Add Components**: Create new components for specific features in `src/app/components/`
3. **Add Routes**: Configure routing in `src/app/app-routing.module.ts`
4. **Styling**: Use SCSS for component-specific styles or update `src/styles.scss` for global styles
5. **Add Backend Features**: Create new models, repositories, and services as needed

---

## License

MIT

