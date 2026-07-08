// // =====================================================================
// //  MyBudget — Assignment 3 entry point (composition root).
// //
// //  >>> YOU WRITE THE DEPENDENCY-INJECTION WIRING HERE (Module 8). <<<
// //
// //  The ConsoleApp UI is provided and depends only on the service
// //  abstractions. Register your implementations with the container so that
// //  ConsoleApp can be resolved. See the "Build specification" in the brief.
// // =====================================================================
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using MyBudget.App;
// using MyBudget.Core;
// using MyBudget.Data;

// var builder = Host.CreateApplicationBuilder(args);

// string dataPath = Path.Combine(AppContext.BaseDirectory, "expenses.json");

// // TODO (Module 8): register your services against their interfaces so the
// // container can construct ConsoleApp. You will need, for example:
// //   - IExpenseStore       -> JsonExpenseStore(dataPath)
// //   - IExpenseRepository  -> ExpenseRepository
// //   - IBudgetService      -> BudgetService
// //   - ConsoleApp          (the UI, so it can be resolved below)
// // Choose appropriate service lifetimes (singleton / scoped / transient).

// using IHost host = builder.Build();

// host.Services.GetRequiredService<ConsoleApp>().Run();

using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBudget.Core;
using MyBudget.Data;

namespace MyBudget.App;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Establish the storage target path 
                string dataPath = Path.Combine(Directory.GetCurrentDirectory(), "expenses.json");

                // --- SERVICE LIFETIME SELECTIONS & REASONINGS ---
                
                // JsonExpenseStore holds static path configurations but no dynamic application states.
                // It can safely live as a Singleton to minimize memory overhead.
                services.AddSingleton<IExpenseStore>(new JsonExpenseStore(dataPath));

                // ExpenseRepository manages our active list state collection throughout runtime.
                // In a single-user console session, a Singleton ensures data remains intact between actions.
                services.AddSingleton<IExpenseRepository, ExpenseRepository>();

                // BudgetService preserves memory flags representing limits set via user interaction.
                // Storing this as a Singleton maintains state visibility across menu commands.
                services.AddSingleton<IBudgetService, BudgetService>();

                // Transient ensures a fresh instance of the application layer is spawned on initiation.
                services.AddTransient<ConsoleApp>();
            })
            .Build();

        var app = host.Services.GetRequiredService<ConsoleApp>();
        app.Run();
    }
}