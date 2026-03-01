# Running the application

The MAUI app talks to the API at `http://localhost:5053`. If the API is not running, you will see connection refused errors and no data.

## Option 1: Visual Studio – multiple startup projects

1. In **Solution Explorer**, right-click the **solution** (top node).
2. Select **Properties** (or **Set Startup Projects**).
3. Under **Startup Project**, choose **Multiple startup projects**.
4. Set **DerivativesPricingApp.Api** to **Start**.
5. Set **DerivativesPricingApp** (MAUI) to **Start**.
6. Use the arrows so that **DerivativesPricingApp.Api** runs first (optional; both can start in parallel).
7. Click **OK**, then press **F5** (or Start). Both the API and the MAUI app will start.

## Option 2: Start the API manually, then the app

1. Set **DerivativesPricingApp.Api** as the single startup project, run it (F5), and leave it running.
2. In another run (or from the same solution), set **DerivativesPricingApp** as startup and run the MAUI app (F5).

## Option 3: Command line

From the solution folder:

```bash
# Terminal 1 – start API
cd DerivativesPricingApp.Api
dotnet run

# Terminal 2 – start MAUI (Windows)
cd DerivativesPricingApp
dotnet build -t:Run -f net9.0-windows10.0.19041.0
```

The API listens on **http://localhost:5053**. The MAUI app uses this address (see `Constants.ApiBaseUrl` in the MAUI project).
