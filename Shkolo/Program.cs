using Shkolo.App.Services;
using Shkolo.Data;
using Shkolo.Data.Services;
using System;


namespace Shkolo.App
{
    class Program
    {
        static void Main(string[] args)
        {

            using var context = new ShkoloContext();
            var importer = new DataImporterService(context);
            var reports = new StudentReportService(context);

            Console.WriteLine("=== Shkolo Management System ===");
            Console.WriteLine("1. Import Data from JSON");
            Console.WriteLine("2. View Student Reports (LINQ Queries)");
            Console.Write("\nSelect an option: ");

            var choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine("Starting import...");
                importer.ImportFromJson("data.json");
            }
            else if (choice == "2")
            {
                reports.PrintAllReports();
            }
            else
            {
                Console.WriteLine("Invalid selection. Press any key to exit.");
            }

            Console.WriteLine("\nTask complete. Press any key to close.");
            Console.ReadKey();
        }
    }
}