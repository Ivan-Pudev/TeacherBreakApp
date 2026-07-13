# TeacherBreakApp

## Project Overview

TeacherBreakApp is an ASP.NET Core MVC application for managing
teachers' annual leave and leave balances.

### Main Roles

-   **Administrator**
    -   Create teacher accounts
    -   Manage leave balances
    -   Review and edit leave entries
-   **Teacher**
    -   Log in
    -   View personal leave balance
    -   View leave information

------------------------------------------------------------------------

# Architecture

``` text
Browser
    │
    ▼
Controllers
    │
    ▼
Services
    │
    ▼
Repositories
    │
    ▼
Entity Framework Core
    │
    ▼
SQL Server
```

  Layer          Responsibility
  -------------- ------------------------------------
  Controllers    Handle HTTP requests and responses
  Services       Business logic
  Repositories   Database operations
  Data Models    Entity definitions
  DbContext      Entity Framework configuration

------------------------------------------------------------------------

# Solution Structure

``` text
TeacherBreakApp.sln

├── TeacherBreakApp
│   └── ASP.NET Core MVC application
│
├── TeacherBreakApp.Data
│   ├── DbContext
│   ├── Identity Seeder
│   ├── Repository
│   └── Migrations
│
├── TeacherBreakApp.Data.Models
│   └── Entity classes
│
├── TeacherBreakApp.Services
│   └── Business logic
│
├── TeacherBreakApp.ViewModels
│   └── View models
│
├── TeacherBreakApp.Web.Infrastructure
│   └── Startup extensions
│
└── TeacherBreakApp.GCommon
    └── Shared constants
```

------------------------------------------------------------------------

# Database

Main entities:

``` text
ApplicationUser
      │
      ▼
LeaveBalance
      │
      ▼
LeaveEntry
```

## ApplicationUser

Represents both administrators and teachers.

Typical properties:

-   Id
-   Email
-   UserName
-   FullName
-   IsDeleted

## LeaveBalance

Stores annual leave information for a teacher.

## LeaveEntry

Represents a leave period with start and end dates.

------------------------------------------------------------------------

# Application Startup

Application startup flow:

``` text
Program.cs
    ↓
Register Services
    ↓
Configure Identity
    ↓
Configure DbContext
    ↓
Run EF Core Migrations
    ↓
Seed Roles
    ↓
Seed Admin User
    ↓
Run Application
```

------------------------------------------------------------------------

# Authentication

Uses ASP.NET Core Identity.

Roles:

-   Admin
-   Teacher

Authentication flow:

``` text
Login
   ↓
Identity Cookie
   ↓
Authorization
   ↓
Controllers
```

------------------------------------------------------------------------

# Controllers

## HomeController

Responsible for redirecting authenticated users.

## AdminController

Main administration features.

Typical responsibilities:

-   Manage teachers
-   Manage leave balances
-   Manage leave entries

## TeacherController

Responsible for teacher dashboard and personal leave information.

------------------------------------------------------------------------

# Services

Business logic is located inside the Services project.

Typical responsibilities:

-   Create teachers
-   Manage leave balances
-   Validate business rules
-   Coordinate repository calls

------------------------------------------------------------------------

# Repository Layer

Repositories communicate directly with Entity Framework.

Typical operations:

-   Get
-   Create
-   Update
-   Delete
-   Save changes

------------------------------------------------------------------------

# Identity Seeder

Runs during application startup.

Creates:

-   Admin role
-   Teacher role
-   Default administrator account

Configuration required:

``` json
"UserConfig": {
  "AdminAcc": {
    "Email": "...",
    "Password": "...",
    "FullName": "..."
  }
}
```

------------------------------------------------------------------------

# Request Flow Example

``` text
Browser
   ↓
Controller
   ↓
Service
   ↓
Repository
   ↓
DbContext
   ↓
SQL Server
```

------------------------------------------------------------------------

# Manual Testing Checklist

-   [ ] Application starts
-   [ ] Database migrations succeed
-   [ ] Roles are seeded
-   [ ] Admin account is created
-   [ ] Login works
-   [ ] Admin dashboard opens
-   [ ] Teacher account creation works
-   [ ] Teacher login works
-   [ ] Leave balances display correctly
-   [ ] Leave editing works
-   [ ] Logout works

------------------------------------------------------------------------

# Suggested Improvements

-   Improve exception messages.
-   Add structured logging.
-   Replace magic strings with constants.
-   Move business validation into services.
-   Add XML documentation.
-   Add unit and integration tests.
-   Introduce Global Query Filters for soft deletes.

------------------------------------------------------------------------

# Development Checklist

Before implementing a feature, identify:

1.  Controller
2.  Service
3.  Repository
4.  Entity
5.  ViewModel
6.  Database migration requirements
7.  Authorization requirements

Following this workflow makes the project easier to maintain and extend.
