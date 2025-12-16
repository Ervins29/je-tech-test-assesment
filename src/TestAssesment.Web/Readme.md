# Movie Search Application

A Blazor Server application that allows users to search for movies using the OMDB API and maintains a history of last 5
searches.

## Technology Stack

- **.NET 10.0** - Framework
- **Blazor Server** - Web UI framework
- **Entity Framework Core** - ORM
- **SQLite** - Database
- **MudBlazor** - UI component library
- **Refit** - HTTP client library for API calls
- **xUnit, Moq, Shouldly** - Testing frameworks

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- OMDB API Key (get one free at https://www.omdbapi.com/apikey.aspx)

## How to Run

### 1. Clone the repository

```bash
cd TestAssesment
```

### 2. Configure the OMDB API Key

**Option A: Using User Secrets (Recommended for Development)**

```bash
cd src/TestAssesment.Web
dotnet user-secrets init
dotnet user-secrets set "OmdbApi:ApiKey" "your-api-key-here"
```

**Option B: Update appsettings.json**

Edit `src/TestAssesment.Web/appsettings.json`:

```json
{
  "OmdbApi": {
    "BaseUrl": "http://www.omdbapi.com",
    "ApiKey": "your-api-key-here"
  }
}
```

### 3. Run the Application

```bash
cd src/TestAssesment.Web
dotnet run
```

## Testing

### Test can be run from the `TestAssesment.Tests` project directory

```bash
cd TestAssesment.Tests
dotnet test
```

## Project Structure

```
TestAssesment/
├── src/
│   ├── TestAssesment.Web/              # Blazor Server UI
│   ├── TestAssesment.Data.DataAccess/  # Entity Framework & Database
│   ├── TestAssesment.Data.Services/    # Business Logic
│   └── TestAssesment.Integrations.Omdb/ # OMDB API Integration
└── TestAssesment.Tests/                # Unit Tests
```

## Database

The application uses SQLite with a local database file (`MovieSearch.db`) that is automatically created on first run.
The database stores:

- Movie search history (title, IMDB ID, timestamp)
- Maximum of 5 entries

## Configuration

Key settings in `appsettings.json`:

- **OmdbApi:BaseUrl** - OMDB API base URL
- **OmdbApi:ApiKey** - Your OMDB API key
- **ConnectionStrings:MovieSearch** - SQLite database path

## Usage

1. Enter a movie title in the search box (minimum 3 characters)
2. Press Enter or click the Search button
3. Click on a movie card to view detailed information
4. Recent searches appear below the search box for quick access