# Derivatives Pricing / Collateral Management Tool

A desktop application for pricing simple financial instruments (options, bonds) and managing collateral and margin calls. It consists of a .NET MAUI client and an ASP.NET Core REST API with a SQLite database.

## Solution structure

| Project | Description |
|--------|-------------|
| **DerivativesPricingApp** | .NET MAUI desktop app (Windows; also targets Android, iOS, Mac Catalyst). UI for pricing, collateral, and margin calls. |
| **DerivativesPricingApp.Api** | ASP.NET Core Web API (.NET 9). Pricing endpoints (Black-Scholes, bonds), CRUD for collateral and margin calls, and read-only data (counterparties, trades, historical prices). Uses EF Core with SQLite. |

## Features

- **Pricing:** European option (Black-Scholes) and coupon bond pricing via API.
- **Collateral:** List, add, edit collateral positions (linked to trades).
- **Margin calls:** List, add margin calls; update status (Pending / Met / Disputed).
- **Data:** Counterparties, trades, and historical prices exposed by the API (read-only in the app).

## Tech stack

- **Client:** .NET 9, MAUI (XAML), `HttpClient` + JSON.
- **API:** ASP.NET Core 9, EF Core, SQLite (no migrations; `EnsureCreated` + seed data).
- **Database:** SQLite file (path from User Secrets: `ConnectionStrings:DatabaseConnection`).

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 (or Rider) with MAUI workload for running the desktop app.

## Running the application

The MAUI app calls the API at **http://localhost:5053**. Both the API and the MAUI app must be running.

See **[RUN.md](RUN.md)** for:

- Setting **multiple startup projects** in Visual Studio (API + MAUI).
- Starting the API and app manually or from the command line.

## Configuration

- **API base URL (MAUI):** `Constants.ApiBaseUrl` in the MAUI project (default `http://localhost:5053`). Change it if the API runs on another host or port.
- **API database:** Connection string in User Secrets for the Api project (`ConnectionStrings:DatabaseConnection`). Default seed creates sample counterparties, trades, collateral, and margin calls on first run.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE.txt) file for details.
