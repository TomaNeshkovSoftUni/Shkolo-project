using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shkolo.Data.DTOs;
using Shkolo.Data.Entities.Enums;
using Shkolo.Data.Services;
using Shkolo.Data.ShkoloDbContext;

namespace Shkolo.App
{
    // This is the App layer! (ProjectName.App)
    // It only handles the console and DTOs, never the raw database Entities
    class Program
    {
        static void Main(string[] args)
        {
            // Set up the connection to our Data project
            using var context = new Shkolo.Data.ShkoloContext();
            var userService = new UserService(context);
            var importer = new DataImporterService(context);

            UserDto loggedInUser = null;

            // Login screen logic
            while (loggedInUser == null)
            {
                Console.Clear();
                Console.WriteLine("=== SHKOLO INFORMATION SYSTEM ===");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Continue as Guest");
                Console.WriteLine("0. Exit");
                Console.Write("\nChoice: ");
                string entry = Console.ReadLine();

                if (entry == "1")
                {
                    Console.Write("Username: "); string u = Console.ReadLine();
                    Console.Write("Password: "); string p = Console.ReadLine();

                    // returns a DTO to keep the App layer looking clean
                    loggedInUser = userService.Authenticate(u, p);

                    if (loggedInUser == null)
                    {
                        Console.WriteLine("Invalid credentials or Blocked! Press any key...");
                        Console.ReadKey();
                    }
                }
                else if (entry == "2")
                {
                    Console.Write("New Username: "); string u = Console.ReadLine();
                    Console.Write("New Password: "); string p = Console.ReadLine();
                    Console.WriteLine(userService.Register(u, p));
                    Console.ReadKey();
                }
                else if (entry == "3")
                {
                    // Guest role has less menu options
                    loggedInUser = new UserDto { Username = "Guest", Role = Role.Guest };
                }
                else if (entry == "0") return;
            }

            // MAIN MENU:
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Logged in as: {loggedInUser.Username} ({loggedInUser.Role})");
                Console.WriteLine(new string('=', 30));

                if (loggedInUser.Role == Role.Administrator)
                {
                    // Admin can ONLY manage users - no data import/export or reports
                    Console.WriteLine("1. List All Users");
                    Console.WriteLine("2. Block User");
                    Console.WriteLine("3. Delete User");
                }
                else if (loggedInUser.Role == Role.RegisteredUser)
                {
                    // Registered Users get JSON import/export and all LINQ reports - no user management
                    Console.WriteLine("1. Import Students (JSON)");
                    Console.WriteLine("2. Export Top Students (JSON)");
                    Console.WriteLine("3. Search Student by Name");
                    Console.WriteLine("4. View Top Students (GPA)");
                    Console.WriteLine("5. View Class Statistics");
                    Console.WriteLine("6. View Popular Subjects");
                    Console.WriteLine("7. View Failing Students");
                    Console.WriteLine("8. View Recent Grades Report");
                }
                else if (loggedInUser.Role == Role.Guest)
                {
                    // Guests only access school classes - read-only
                    Console.WriteLine("1. View School Classes");
                }

                Console.WriteLine("0. Logout/Exit");
                Console.Write("\nSelect: ");
                string choice = Console.ReadLine();
                if (choice == "0") break;

                HandleMenuSelection(choice, loggedInUser, userService, importer, context);

                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }
        }

        // This method separates the menu logic from the Main loop
        static void HandleMenuSelection(string choice, UserDto user, UserService uService, DataImporterService importer, Shkolo.Data.ShkoloContext context)
        {
            if (user.Role == Role.Administrator)
            {
                if (choice == "1")
                {
                    var users = uService.GetAllUsers();
                    foreach (var u in users) Console.WriteLine($"- {u.Username} [{u.Role}] Status: {u.Status}");
                }
                else if (choice == "2")
                {
                    Console.Write("Username to block: ");
                    string target = Console.ReadLine();
                    if (!string.IsNullOrEmpty(target))
                        Console.WriteLine(uService.ChangeUserStatus(target, UserStatus.Blocked) ? "User blocked." : "User not found.");
                }
                else if (choice == "3")
                {
                    Console.Write("Username to delete: ");
                    string target = Console.ReadLine();
                    if (!string.IsNullOrEmpty(target))
                        Console.WriteLine(uService.DeleteUser(target) ? "User deleted." : "Failed.");
                }
            }
            else if (user.Role == Role.RegisteredUser)
            {
                switch (choice)
                {
                    case "1":
                        // JSON Import Requirement
                        Console.Write("Enter path (e.g. data.json): ");
                        string path = Console.ReadLine();
                        Console.WriteLine(importer.ImportFromJson(path));
                        break;
                    case "2":
                        // JSON Export Requirement
                        Console.WriteLine(importer.ExportTopStudentsToJson("top_students.json"));
                        break;
                    case "3":
                        // LINQ Query: Search by criteria
                        Console.Write("Search name: ");
                        var search = importer.SearchStudentsByName(Console.ReadLine());
                        search.ForEach(s => Console.WriteLine($"{s.FullName} ({s.ClassName})"));
                        break;
                    case "4":
                        // LINQ Query: Aggregation (GPA)
                        var tops = importer.GetTopStudents(5);
                        tops.ForEach(t => Console.WriteLine($"{t.FullName}: {t.AverageGrade:F2}"));
                        break;
                    case "5":
                        // LINQ Query: Filtering
                        Console.Write("Class: ");
                        var classList = importer.GetStudentsByClass(Console.ReadLine());
                        classList.ForEach(s => Console.WriteLine($"- {s.FullName}"));
                        break;
                    case "6":
                        // LINQ Query: Grouping
                        var popular = importer.GetPopularSubjects();
                        foreach (var p in popular) Console.WriteLine($"{p.Name}: {p.StudentCount} students");
                        break;
                    case "7":
                        // LINQ Query: Selection
                        importer.GetFailingStudents().ForEach(s => Console.WriteLine($"ALERT: {s.FullName} has low grades!"));
                        break;
                    case "8":
                        // LINQ Query: Date filtering
                        var recent = importer.GetRecentGrades();
                        foreach (var r in recent)
                            Console.WriteLine($"[{r.Date:d}] {r.Subject}: {r.Student} got {r.Value}");
                        break;
                }
            }
            else if (user.Role == Role.Guest && choice == "1")
            {
                // Read-only info for Guest
                var classes = importer.GetPublicClasses();
                if (!classes.Any()) Console.WriteLine("No classes found.");
                classes.ForEach(c => Console.WriteLine($" * {c}"));
            }
        }
    }
}