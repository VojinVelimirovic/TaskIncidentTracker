# Task Incident Tracker

A full-stack task management application built with React, .NET, and PostgreSQL.

## Technologies Used

### Frontend

- **React** - UI framework
- **React Router** - Client-side routing
- **Vite** - Build tool and dev server

### Backend

- **.NET 8** - Web API framework
- **Entity Framework Core** - ORM for database operations
- **JWT Authentication** - Secure token-based authentication
- **Npgsql** - PostgreSQL database provider

### Database

- **PostgreSQL** - Relational database (hosted on Supabase)
- **Supabase** - Cloud database platform

### Deployment

- **Docker** - Containerization
- **Docker Compose** - Multi-container orchestration

## How the Application Works

### User Roles

The application has three distinct user roles, each with different permissions:

1. **User (Role: 2)** - Default role
   - Can create new tasks
   - Can view tasks assigned to them
   - Can view their own created tasks

2. **Manager (Role: 1)**
   - All User permissions
   - Can assign tasks to other users
   - Can change task status (Pending, InProgress, Completed)
   - Can view all tasks in the system

3. **Admin (Role: 0)**
   - All User permissions
   - Can change user roles
   - Full system access

### Task Management Flow

1. Any user can create a task with a title, description, and priority level
2. Managers can assign tasks to one or more users
3. Managers can update task status as work progresses
4. Users can view their assigned tasks and track progress

## Setup Instructions

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop) installed
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed (for local development)
- [Node.js](https://nodejs.org/) (v18 or higher) installed (for local development)
- A [Supabase](https://supabase.com) account

### 1. Clone the Repository

```bash
git clone <your-repo-url>
cd TaskIncidentTracker
```

### 2. Set Up Supabase Database

1. Go to [Supabase](https://supabase.com) and sign up/login
2. Create a new project:
   - Project name: `task-incident-tracker`
   - Database password: Choose a secure password (e.g., `TaskTracker2025!`)
   - Region: Choose the closest region to you
3. Wait 2-3 minutes for the database to provision
4. Get your connection string:
   - Go to **Project Settings** (gear icon)
   - Click **Database**
   - Click **Connection string**
   - Select **Session pooler** mode
   - Copy the connection string

### 3. Configure Environment Variables

1. Copy the example environment file:

   ```bash
   cp .env.example .env
   ```

   Use copy instead of cp if you're on windows.

2. Edit `.env` and update the Id= portion of your DATABASE_URL with your Supabase connection string from earlier:

   ```
   JWT_SECRET=ThisIsASuperSecretKeyThatIsAtLeast32Chars!
   JWT_ISSUER=TaskIncidentTracker
   JWT_AUDIENCE=TaskIncidentTrackerUsers
   DATABASE_URL=User Id=postgres.YOUR_PROJECT_ID;Password=YOUR_PASSWORD;Server=aws-X-region.pooler.supabase.com;Port=5432;Database=postgres
   ASPNETCORE_ENVIRONMENT=Development
   FRONTEND_URL=http://localhost:3000
   ```

3. Create frontend environment file:
   ```bash
   cd task-incident-tracker-frontend
   cp .env.example .env
   cd ..
   ```
   Use copy instead of cp if you're on windows.

### 4. Run the Application

Build and start all containers:

```bash
docker compose up -d --build
```

The application will be available at:

- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000

### 5. Set Up the First Admin User

**IMPORTANT**: You must manually set up the first admin user to manage other users' roles.

1. Register a new user through the application at http://localhost:3000
2. Go to your Supabase dashboard
3. Click **Table Editor** in the left sidebar
4. Click on the **Users** table
5. Find your newly registered user
6. Click on the **Role** cell for that user
7. Change the value from `2` to `0`
8. Press Enter to save

Alternatively, use the SQL Editor:

1. Click **SQL Editor** in the Supabase sidebar
2. Click **New query**
3. Paste this SQL (replace `your-username` with your actual username):
   ```sql
   UPDATE "Users"
   SET "Role" = 0
   WHERE "Username" = 'your-username';
   ```
4. Click **Run** or press Ctrl+Enter

You can now log in with this user and access the Admin panel to manage other users' roles.

### 6. Verify Everything Works

1. Log in with your admin user
2. Navigate to the Admin panel
3. Try creating tasks, assigning them, and changing their status
4. Register additional users and test role-based permissions

## Development

### Running Locally Without Docker

**Backend:**

```bash
cd TaskIncidentTracker.Api
dotnet restore
dotnet run
```

**Frontend:**

```bash
cd task-incident-tracker-frontend
npm install
npm run dev
```

### Stopping the Application

```bash
docker compose down
```

To remove all data and start fresh:

```bash
docker compose down -v
```

## Project Structure

```
TaskIncidentTracker/
├── TaskIncidentTracker.Api/       # .NET Web API
│   ├── Controllers/               # API endpoints
│   ├── Services/                  # Business logic
│   ├── Models/                    # Data models
│   ├── Data/                      # Database context
│   └── Migrations/                # EF Core migrations
├── task-incident-tracker-frontend/ # React frontend
│   ├── src/
│   │   ├── components/            # React components
│   │   ├── services/              # API services
│   │   └── utils/                 # Utility functions
│   └── public/                    # Static assets
├── docker-compose.yml             # Docker orchestration
└── .env                           # Environment variables
```

## Troubleshooting

### Database Connection Issues

If you see database connection errors:

1. Verify your Supabase connection string is correct
2. Ensure you're using the **Session pooler** connection string (not Direct connection)
3. Check that your Supabase project is active

### Port Already in Use

If ports 3000 or 5000 are already in use, you can change them in `docker-compose.yml`:

```yaml
ports:
  - "YOUR_PORT:8080" # For API
  - "YOUR_PORT:80" # For frontend
```

### Frontend Can't Connect to API

Make sure the `VITE_API_URL` in `task-incident-tracker-frontend/.env` matches your backend URL.

## License

This project is for educational/portfolio purposes.
