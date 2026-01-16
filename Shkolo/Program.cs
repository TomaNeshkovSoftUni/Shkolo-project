using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Shkolo.Data.Services;
using Shkolo.Data;
using Shkolo.Data.DTOs;
using Shkolo.Data.Entities;
using Shkolo.Data.Entities.Enums;

namespace Shkolo.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var userService = new UserService();
            using var context = new ShkoloContext();
            var importer = new DataImporterService(context);
            UserDto loggedInUser = null;

            // --- 1. START SCREEN (Login / Register / Guest) ---
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
                    loggedInUser = userService.Authenticate(u, p);
                    if (loggedInUser == null) { Console.WriteLine("Invalid credentials or Blocked!"); Console.ReadKey(); }
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
                    loggedInUser = new UserDto { Username = "Guest", Role = Role.Guest };
                }
                else if (entry == "0") return;
            }

            // --- 2. MAIN MENU LOOP ---
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Logged in as: {loggedInUser.Username} ({loggedInUser.Role})");
                Console.WriteLine(new string('=', 30));

                if (loggedInUser.Role == Role.Administrator)
                {
                    Console.WriteLine("1. List All Users");
                    Console.WriteLine("2. Block User");
                    Console.WriteLine("3. Delete User");
                }
                else if (loggedInUser.Role == Role.RegisteredUser)
                {
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
                    Console.WriteLine("1. View School Classes (Read-Only)");
                }

                Console.WriteLine("0. Logout/Exit");
                Console.Write("\nSelect: ");
                string choice = Console.ReadLine();
                if (choice == "0") break;

                HandleMenuSelection(choice, loggedInUser, userService, importer);
                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }
        }

        static void HandleMenuSelection(string choice, UserDto user, UserService uService, DataImporterService importer)
        {
            // --- ADMINISTRATOR LOGIC ---
            if (user.Role == Role.Administrator)
            {
                if (choice == "1")
                {
                    var users = uService.GetAllUsers();
                    foreach (var u in users) Console.WriteLine($"- {u.Username} [{u.Role}] Status: {u.Status}");
                }
                else if (choice == "2")
                {
                    Console.Write("Username to block: "); string target = Console.ReadLine();
                    Console.WriteLine(uService.ChangeUserStatus(target, UserStatus.Blocked) ? "Blocked." : "Failed.");
                }
                else if (choice == "3")
                {
                    Console.Write("Username to delete: "); string target = Console.ReadLine();
                    Console.WriteLine(uService.DeleteUser(target) ? "Deleted." : "Failed.");
                }
            }
            // --- REGISTERED USER LOGIC ---
            else if (user.Role == Role.RegisteredUser)
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter JSON path: ");
                        Console.WriteLine(importer.ImportFromJson(Console.ReadLine()));
                        break;
                    case "2":
                        Console.WriteLine(importer.ExportTopStudentsToJson("top_students.json"));
                        break;
                    case "3":
                        Console.Write("Enter name: ");
                        var search = importer.SearchStudentsByName(Console.ReadLine());
                        search.ForEach(s => Console.WriteLine($"{s.FirstName} {s.LastName} ({s.ClassName})"));
                        break;
                    case "4":
                        var tops = importer.GetTopStudents(5);
                        tops.ForEach(t => Console.WriteLine($"{t.FullName}: {t.AverageGrade:F2}"));
                        break;
                    case "5":
                        Console.Write("Enter Class (e.g. 12A): ");
                        var classList = importer.GetStudentsByClass(Console.ReadLine());
                        classList.ForEach(s => Console.WriteLine($"{s.FirstName} {s.LastName}"));
                        break;
                    case "6":
                        var popular = importer.GetPopularSubjects();
                        foreach (var p in popular) Console.WriteLine($"{p.Name}: {p.StudentCount} students");
                        break;
                    case "7":
                        importer.GetFailingStudents().ForEach(s => Console.WriteLine($"FAILING: {s.FirstName} {s.LastName}"));
                        break;
                    case "8":
                        var recent = importer.GetRecentGrades();
                        foreach (var r in recent) Console.WriteLine($"[{r.Subject}] {r.Student}: {r.Value}");
                        break;
                }
            }
            // --- GUEST LOGIC ---
            else if (user.Role == Role.Guest && choice == "1")
            {
                // Simple read-only logic
                Console.WriteLine("Displaying public school classes...");
            }
        }
    }
}