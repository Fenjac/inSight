using inSight.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inSight.API.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Proveri da li već ima podataka
            if (await context.Users.AnyAsync())
            {
                Console.WriteLine("⚠️ Users already exist, skipping seed.");
                return; // Baza već ima podatke
            }

            Console.WriteLine("🌱 DbSeeder.SeedAsync STARTED!");
            Console.WriteLine("✅ No users found, proceeding with seed...");

            // 1. KREIRAJ SYSTEM ROLES
            var systemRoles = new List<SystemRole>
            {
                new SystemRole { Name = "Employee", Description = "Regular employee", PermissionLevel = 1 },
                new SystemRole { Name = "TeamLead", Description = "Team leader", PermissionLevel = 2 },
                new SystemRole { Name = "HR", Description = "Human resources", PermissionLevel = 3 },
                new SystemRole { Name = "CTO", Description = "Chief Technology Officer", PermissionLevel = 4 },
                new SystemRole { Name = "CEO", Description = "Chief Executive Officer", PermissionLevel = 5 }
            };
            await context.SystemRoles.AddRangeAsync(systemRoles);
            await context.SaveChangesAsync();

            var employeeRole = systemRoles.First(r => r.Name == "Employee");
            var teamLeadRole = systemRoles.First(r => r.Name == "TeamLead");
            var hrRole = systemRoles.First(r => r.Name == "HR");
            var ctoRole = systemRoles.First(r => r.Name == "CTO");
            var ceoRole = systemRoles.First(r => r.Name == "CEO");

            // 2. KREIRAJ JOB ROLES
            var jobRoles = new List<Role>
            {
                new Role { Name = "Developer", Description = "Software Developer" },
                new Role { Name = "QA", Description = "Quality Assurance" },
                new Role { Name = "Application Engineer", Description = "Application Engineer" }
            };
            await context.Roles.AddRangeAsync(jobRoles);
            await context.SaveChangesAsync();

            var devRole = jobRoles.First(r => r.Name == "Developer");
            var qaRole = jobRoles.First(r => r.Name == "QA");
            var appRole = jobRoles.First(r => r.Name == "Application Engineer");

            // 3. KREIRAJ RANKS (za Developer, QA, Application Engineer)
            var ranks = new List<Rank>();

            // Praktikant ranks (P1-P3)
            for (int i = 1; i <= 3; i++)
            {
                foreach (var role in jobRoles)
                {
                    ranks.Add(new Rank
                    {
                        RoleId = role.Id,
                        Code = $"P{i}",
                        Name = $"Praktikant {i}",
                        MinSalary = 30000 + (i * 5000),
                        MaxSalary = 40000 + (i * 5000),
                        MinScore = 1.0m + (i * 0.5m),
                        MaxScore = 1.5m + (i * 0.5m),
                        OrderIndex = i
                    });
                }
            }

            // Junior ranks (J1-J6)
            for (int i = 1; i <= 6; i++)
            {
                foreach (var role in jobRoles)
                {
                    ranks.Add(new Rank
                    {
                        RoleId = role.Id,
                        Code = $"J{i}",
                        Name = $"Junior {i}",
                        MinSalary = 50000 + (i * 5000),
                        MaxSalary = 60000 + (i * 5000),
                        MinScore = 2.0m + (i * 0.3m),
                        MaxScore = 2.3m + (i * 0.3m),
                        OrderIndex = 10 + i
                    });
                }
            }

            // Medior ranks (M1-M6)
            for (int i = 1; i <= 6; i++)
            {
                foreach (var role in jobRoles)
                {
                    ranks.Add(new Rank
                    {
                        RoleId = role.Id,
                        Code = $"M{i}",
                        Name = $"Medior {i}",
                        MinSalary = 80000 + (i * 8000),
                        MaxSalary = 90000 + (i * 8000),
                        MinScore = 3.0m + (i * 0.2m),
                        MaxScore = 3.2m + (i * 0.2m),
                        OrderIndex = 20 + i
                    });
                }
            }

            // Senior ranks (S1-S6)
            for (int i = 1; i <= 6; i++)
            {
                foreach (var role in jobRoles)
                {
                    ranks.Add(new Rank
                    {
                        RoleId = role.Id,
                        Code = $"S{i}",
                        Name = $"Senior {i}",
                        MinSalary = 120000 + (i * 10000),
                        MaxSalary = 135000 + (i * 10000),
                        MinScore = 4.0m + (i * 0.15m),
                        MaxScore = 4.15m + (i * 0.15m),
                        OrderIndex = 30 + i
                    });
                }
            }

            // Expert ranks (E1-E3)
            for (int i = 1; i <= 3; i++)
            {
                foreach (var role in jobRoles)
                {
                    ranks.Add(new Rank
                    {
                        RoleId = role.Id,
                        Code = $"E{i}",
                        Name = $"Expert {i}",
                        MinSalary = 180000 + (i * 15000),
                        MaxSalary = 200000 + (i * 15000),
                        MinScore = 4.7m + (i * 0.1m),
                        MaxScore = 4.8m + (i * 0.1m),
                        OrderIndex = 40 + i
                    });
                }
            }

            await context.Ranks.AddRangeAsync(ranks);
            await context.SaveChangesAsync();

            // 4. KREIRAJ TIMOVE - 8 timova
            var teams = new List<Team>
            {
                new Team { Name = "Backend", Description = "Backend development team" },
                new Team { Name = "Backend Boomerang", Description = "Backend Boomerang team" },
                new Team { Name = "Frontend", Description = "Frontend development team" },
                new Team { Name = "Frontend Boomerang", Description = "Frontend Boomerang team" },
                new Team { Name = "QA", Description = "Quality Assurance team" },
                new Team { Name = "Application", Description = "Application development team" },
                new Team { Name = "DevOps", Description = "DevOps team" },
                new Team { Name = "AI", Description = "AI/ML team" }
            };
            await context.Teams.AddRangeAsync(teams);
            await context.SaveChangesAsync();

            var backendTeam = teams.First(t => t.Name == "Backend");
            var backendBoomerangTeam = teams.First(t => t.Name == "Backend Boomerang");
            var frontendTeam = teams.First(t => t.Name == "Frontend");
            var frontendBoomerangTeam = teams.First(t => t.Name == "Frontend Boomerang");
            var qaTeam = teams.First(t => t.Name == "QA");
            var applicationTeam = teams.First(t => t.Name == "Application");
            var devopsTeam = teams.First(t => t.Name == "DevOps");
            var aiTeam = teams.First(t => t.Name == "AI");

            // 5. KREIRAJ KVARTALE
            var quarters = new List<Quarter>
            {
                new Quarter
                {
                    Year = 2024,
                    QuarterNumber = 3,
                    StartDate = new DateTime(2024, 7, 1),
                    EndDate = new DateTime(2024, 9, 30),
                    IsActive = false,
                    IsLocked = true
                },
                new Quarter
                {
                    Year = 2024,
                    QuarterNumber = 4,
                    StartDate = new DateTime(2024, 10, 1),
                    EndDate = new DateTime(2024, 12, 31),
                    IsActive = true,
                    IsLocked = false
                }
            };
            await context.Quarters.AddRangeAsync(quarters);
            await context.SaveChangesAsync();

            // 6. KREIRAJ MANAGEMENT
            var ceo = new User
            {
                FirstName = "Marko",
                LastName = "Marković",
                Email = "marko.markovic@init.rs",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = ceoRole.Id,
                IsActive = true
            };

            var cto = new User
            {
                FirstName = "Ana",
                LastName = "Anić",
                Email = "ana.anic@init.rs",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = ctoRole.Id,
                IsActive = true
            };

            var hr = new User
            {
                FirstName = "Jelena",
                LastName = "Nikolić",
                Email = "jelena.nikolic@init.rs",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = hrRole.Id,
                IsActive = true
            };

            await context.Users.AddRangeAsync(new[] { ceo, cto, hr });
            await context.SaveChangesAsync();

            // 7. KREIRAJ ZAPOSLENE iz Excel liste
            var employees = new List<User>();

            // Helper funkcija za pronalazak ranga
            Rank GetRank(string rankCode, Guid roleId)
            {
                return ranks.First(r => r.Code == rankCode && r.RoleId == roleId);
            }

            // DEVOPS TIM (2 zaposlena) - Lead: Apatović Aleksandar
            employees.Add(new User
            {
                FirstName = "Aleksandar",
                LastName = "Apatović",
                Email = "aapatovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = teamLeadRole.Id, // TEAM LEAD
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M1", devRole.Id).Id,
                TeamId = devopsTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Marko",
                LastName = "Apatović",
                Email = "mapatovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("J3", devRole.Id).Id,
                TeamId = devopsTeam.Id,
                IsActive = true
            });

            // BACKEND TIM - Lead: Sabo Robert
            employees.Add(new User
            {
                FirstName = "Robert",
                LastName = "Sabo",
                Email = "rsabo@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = teamLeadRole.Id, // TEAM LEAD
                RoleId = devRole.Id,
                CurrentRankId = GetRank("S1", devRole.Id).Id,
                TeamId = backendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Tomislav",
                LastName = "Kralj",
                Email = "tkralj@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M2", devRole.Id).Id,
                TeamId = backendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Aleksa",
                LastName = "Rokvić",
                Email = "arokvic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M1", devRole.Id).Id,
                TeamId = backendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Igor",
                LastName = "Stojanović",
                Email = "istojanovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M3", devRole.Id).Id,
                TeamId = backendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Lazar",
                LastName = "Stojčević",
                Email = "lstojcevic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M3", devRole.Id).Id,
                TeamId = backendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Stefan",
                LastName = "Vasić",
                Email = "svasic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("S4", devRole.Id).Id,
                TeamId = backendTeam.Id,
                IsActive = true
            });

            // BACKEND BOOMERANG TIM - Lead: Basarić Milica
            employees.Add(new User
            {
                FirstName = "Milica",
                LastName = "Basarić",
                Email = "mbasaric@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = teamLeadRole.Id, // TEAM LEAD
                RoleId = devRole.Id,
                CurrentRankId = GetRank("S2", devRole.Id).Id,
                TeamId = backendBoomerangTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Jovana",
                LastName = "Nukić",
                Email = "jnukic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M3", devRole.Id).Id,
                TeamId = backendBoomerangTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Miloš",
                LastName = "Obradović",
                Email = "mobradovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("J4", devRole.Id).Id,
                TeamId = backendBoomerangTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Đorđe",
                LastName = "Stanisavljević",
                Email = "djstanisavljevic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("J3", devRole.Id).Id,
                TeamId = backendBoomerangTeam.Id,
                IsActive = true
            });

            // AI TIM - Lead: Babić Jela
            employees.Add(new User
            {
                FirstName = "Jela",
                LastName = "Babić",
                Email = "jbabic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = teamLeadRole.Id, // TEAM LEAD
                RoleId = devRole.Id,
                CurrentRankId = GetRank("S1", devRole.Id).Id,
                TeamId = aiTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Iva",
                LastName = "Đerić",
                Email = "idjeric@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("P2", devRole.Id).Id,
                TeamId = aiTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Aleksa",
                LastName = "Naglić",
                Email = "anaglic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("P1", devRole.Id).Id,
                TeamId = aiTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Manojlo",
                LastName = "Novković",
                Email = "mnovkovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("P1", devRole.Id).Id,
                TeamId = aiTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Anja",
                LastName = "Pantović",
                Email = "apantovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("S1", devRole.Id).Id,
                TeamId = aiTeam.Id,
                IsActive = true
            });

            // QA TIM - Lead: Zimonjić Marko
            employees.Add(new User
            {
                FirstName = "Marko",
                LastName = "Zimonjić",
                Email = "mzimonjic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = teamLeadRole.Id, // TEAM LEAD
                RoleId = qaRole.Id,
                CurrentRankId = GetRank("J5", qaRole.Id).Id,
                TeamId = qaTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Smiljana",
                LastName = "Beodranskih",
                Email = "sbeodranskih@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = qaRole.Id,
                CurrentRankId = GetRank("J4", qaRole.Id).Id,
                TeamId = qaTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Slobodan",
                LastName = "Blagojev",
                Email = "sblagojev@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = qaRole.Id,
                CurrentRankId = GetRank("P1", qaRole.Id).Id,
                TeamId = qaTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Marko",
                LastName = "Mitrović",
                Email = "mmitrovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = qaRole.Id,
                CurrentRankId = GetRank("J4", qaRole.Id).Id,
                TeamId = qaTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Dragan",
                LastName = "Nikolić",
                Email = "dnikolic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = qaRole.Id,
                CurrentRankId = GetRank("J2", qaRole.Id).Id,
                TeamId = qaTeam.Id,
                IsActive = true
            });

            // FRONTEND TIM - Lead: Matić Dušica
            employees.Add(new User
            {
                FirstName = "Dušica",
                LastName = "Matić",
                Email = "dmatic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = teamLeadRole.Id, // TEAM LEAD
                RoleId = qaRole.Id,
                CurrentRankId = GetRank("S4", qaRole.Id).Id,
                TeamId = frontendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Mihailo",
                LastName = "Jeremić",
                Email = "mjeremic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("P2", devRole.Id).Id,
                TeamId = frontendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Milica",
                LastName = "Koval",
                Email = "mkoval@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M5", devRole.Id).Id,
                TeamId = frontendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Andrej",
                LastName = "Lazić",
                Email = "alazic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("J3", devRole.Id).Id,
                TeamId = frontendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Ognjen",
                LastName = "Rodić",
                Email = "orodic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("J1", devRole.Id).Id,
                TeamId = frontendTeam.Id,
                IsActive = true
            });

            // FRONTEND BOOMERANG TIM - Lead: Glušac Dušan
            employees.Add(new User
            {
                FirstName = "Dušan",
                LastName = "Glušac",
                Email = "dglusac@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = teamLeadRole.Id, // TEAM LEAD
                RoleId = devRole.Id,
                CurrentRankId = GetRank("S2", devRole.Id).Id,
                TeamId = frontendBoomerangTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Aleksa",
                LastName = "Dmitrović",
                Email = "admitrovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = qaRole.Id,
                CurrentRankId = GetRank("P1", qaRole.Id).Id,
                TeamId = frontendBoomerangTeam.Id,
                IsActive = true
            });

            // APPLICATION TIM - Lead: Savanović Živojin
            employees.Add(new User
            {
                FirstName = "Živojin",
                LastName = "Savanović",
                Email = "zsavanovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = teamLeadRole.Id, // TEAM LEAD
                RoleId = appRole.Id,
                CurrentRankId = GetRank("M5", appRole.Id).Id,
                TeamId = applicationTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Željka",
                LastName = "Kokotović",
                Email = "zkokotovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = appRole.Id,
                CurrentRankId = GetRank("J2", appRole.Id).Id,
                TeamId = applicationTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Anđela",
                LastName = "Kopanja",
                Email = "akopanja@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = appRole.Id,
                CurrentRankId = GetRank("J1", appRole.Id).Id,
                TeamId = applicationTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Ivan",
                LastName = "Žarđa",
                Email = "izardja@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = appRole.Id,
                CurrentRankId = GetRank("M6", appRole.Id).Id,
                TeamId = applicationTeam.Id,
                IsActive = true
            });

            await context.Users.AddRangeAsync(employees);
            await context.SaveChangesAsync();

            // 8. POSTAVI TEAM LEADS za svaki tim
            backendTeam.TeamLeadId = employees.First(e => e.Email == "rsabo@industrial-it.software").Id;
            backendBoomerangTeam.TeamLeadId = employees.First(e => e.Email == "mbasaric@industrial-it.software").Id;
            frontendTeam.TeamLeadId = employees.First(e => e.Email == "dmatic@industrial-it.software").Id;
            frontendBoomerangTeam.TeamLeadId = employees.First(e => e.Email == "dglusac@industrial-it.software").Id;
            qaTeam.TeamLeadId = employees.First(e => e.Email == "mzimonjic@industrial-it.software").Id;
            applicationTeam.TeamLeadId = employees.First(e => e.Email == "zsavanovic@industrial-it.software").Id;
            devopsTeam.TeamLeadId = employees.First(e => e.Email == "aapatovic@industrial-it.software").Id;
            aiTeam.TeamLeadId = employees.First(e => e.Email == "jbabic@industrial-it.software").Id;

            await context.SaveChangesAsync();

            // 9. KREIRAJ EVALUATION CATEGORIES I QUESTIONS
            await SeedEvaluationQuestionsAsync(context);
            // 10. KREIRAJ EVALUACIJE za oba kvartala
            await SeedEvaluationsAsync(context, quarters, employees);

            Console.WriteLine("✅ Baza je uspešno popunjena podacima!");
            Console.WriteLine($"   - {systemRoles.Count} system roles");
            Console.WriteLine($"   - {jobRoles.Count} job roles");
            Console.WriteLine($"   - {ranks.Count} ranks");
            Console.WriteLine($"   - {teams.Count} teams");
            Console.WriteLine($"   - {quarters.Count} quarters");
            Console.WriteLine($"   - {3 + employees.Count} users");
            Console.WriteLine($"   - 8 team leads configured");
        }

        private static async Task SeedEvaluationQuestionsAsync(ApplicationDbContext context)
        {
            var categories = new List<EvaluationCategory>();

            // DEV Kategorije
            var devCategories = new[]
            {
                new EvaluationCategory { Name = "Tehnička kompetentnost", QuestionnaireType = QuestionnaireType.DEV, OrderIndex = 1, IsManagementOnly = false },
                new EvaluationCategory { Name = "Timski rad", QuestionnaireType = QuestionnaireType.DEV, OrderIndex = 2, IsManagementOnly = false },
                new EvaluationCategory { Name = "Komunikacija", QuestionnaireType = QuestionnaireType.DEV, OrderIndex = 3, IsManagementOnly = false }
            };
            categories.AddRange(devCategories);

            // QA Kategorije
            var qaCategories = new[]
            {
                new EvaluationCategory { Name = "Testing skills", QuestionnaireType = QuestionnaireType.QA, OrderIndex = 1, IsManagementOnly = false },
                new EvaluationCategory { Name = "Attention to detail", QuestionnaireType = QuestionnaireType.QA, OrderIndex = 2, IsManagementOnly = false },
                new EvaluationCategory { Name = "Komunikacija", QuestionnaireType = QuestionnaireType.QA, OrderIndex = 3, IsManagementOnly = false }
            };
            categories.AddRange(qaCategories);

            // APP Kategorije
            var appCategories = new[]
            {
                new EvaluationCategory { Name = "Mobile development", QuestionnaireType = QuestionnaireType.APP, OrderIndex = 1, IsManagementOnly = false },
                new EvaluationCategory { Name = "UI/UX razumevanje", QuestionnaireType = QuestionnaireType.APP, OrderIndex = 2, IsManagementOnly = false },
                new EvaluationCategory { Name = "Timski rad", QuestionnaireType = QuestionnaireType.APP, OrderIndex = 3, IsManagementOnly = false }
            };
            categories.AddRange(appCategories);

            // DevOps Kategorije
            var devOpsCategories = new[]
            {
                new EvaluationCategory { Name = "Infrastructure knowledge", QuestionnaireType = QuestionnaireType.DevOps, OrderIndex = 1, IsManagementOnly = false },
                new EvaluationCategory { Name = "Problem solving", QuestionnaireType = QuestionnaireType.DevOps, OrderIndex = 2, IsManagementOnly = false },
                new EvaluationCategory { Name = "Dokumentacija", QuestionnaireType = QuestionnaireType.DevOps, OrderIndex = 3, IsManagementOnly = false }
            };
            categories.AddRange(devOpsCategories);

            // TeamLead Kategorije
            var teamLeadCategories = new[]
            {
                new EvaluationCategory { Name = "Liderstvo", QuestionnaireType = QuestionnaireType.TeamLead, OrderIndex = 1, IsManagementOnly = false },
                new EvaluationCategory { Name = "Komunikacija sa timom", QuestionnaireType = QuestionnaireType.TeamLead, OrderIndex = 2, IsManagementOnly = false },
                new EvaluationCategory { Name = "Podrška i mentorstvo", QuestionnaireType = QuestionnaireType.TeamLead, OrderIndex = 3, IsManagementOnly = false }
            };
            categories.AddRange(teamLeadCategories);

            await context.EvaluationCategories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // Dodaj pitanja
            var questions = new List<EvaluationQuestion>();

            // DEV pitanja
            questions.AddRange(new[]
            {
                new EvaluationQuestion { CategoryId = devCategories[0].Id, QuestionText = "Da li zaposleni pokazuje duboko razumevanje tehnologija?", OrderIndex = 1 },
                new EvaluationQuestion { CategoryId = devCategories[0].Id, QuestionText = "Da li je kod čitljiv i dobro dokumentovan?", OrderIndex = 2 },
                new EvaluationQuestion { CategoryId = devCategories[1].Id, QuestionText = "Da li dobro komunicira sa timom?", OrderIndex = 1 },
                new EvaluationQuestion { CategoryId = devCategories[1].Id, QuestionText = "Da li pomaže kolegama kada je potrebno?", OrderIndex = 2 },
                new EvaluationQuestion { CategoryId = devCategories[2].Id, QuestionText = "Da li jasno izražava svoje ideje?", OrderIndex = 1 }
            });

            // QA pitanja
            questions.AddRange(new[]
            {
                new EvaluationQuestion { CategoryId = qaCategories[0].Id, QuestionText = "Da li detaljno testira funkcionalnosti?", OrderIndex = 1 },
                new EvaluationQuestion { CategoryId = qaCategories[0].Id, QuestionText = "Da li pronalazi edge case-ove?", OrderIndex = 2 },
                new EvaluationQuestion { CategoryId = qaCategories[1].Id, QuestionText = "Da li prati detalje u dokumentaciji?", OrderIndex = 1 },
                new EvaluationQuestion { CategoryId = qaCategories[2].Id, QuestionText = "Da li jasno reportuje bugove?", OrderIndex = 1 }
            });

            // APP pitanja
            questions.AddRange(new[]
            {
                new EvaluationQuestion { CategoryId = appCategories[0].Id, QuestionText = "Da li razume mobile platforme (iOS/Android)?", OrderIndex = 1 },
                new EvaluationQuestion { CategoryId = appCategories[0].Id, QuestionText = "Da li optimizuje performanse aplikacije?", OrderIndex = 2 },
                new EvaluationQuestion { CategoryId = appCategories[1].Id, QuestionText = "Da li implementira UI prema dizajnu?", OrderIndex = 1 },
                new EvaluationQuestion { CategoryId = appCategories[2].Id, QuestionText = "Da li sarađuje sa backend timom?", OrderIndex = 1 }
            });

            // DevOps pitanja
            questions.AddRange(new[]
            {
                new EvaluationQuestion { CategoryId = devOpsCategories[0].Id, QuestionText = "Da li dobro poznaje cloud infrastrukturu?", OrderIndex = 1 },
                new EvaluationQuestion { CategoryId = devOpsCategories[0].Id, QuestionText = "Da li održava CI/CD pipeline?", OrderIndex = 2 },
                new EvaluationQuestion { CategoryId = devOpsCategories[1].Id, QuestionText = "Da li brzo rešava production probleme?", OrderIndex = 1 },
                new EvaluationQuestion { CategoryId = devOpsCategories[2].Id, QuestionText = "Da li dokumentuje infrastrukturu?", OrderIndex = 1 }
            });

            // TeamLead pitanja
            questions.AddRange(new[]
            {
                new EvaluationQuestion { CategoryId = teamLeadCategories[0].Id, QuestionText = "Da li daje jasne smernice i ciljeve?", OrderIndex = 1 },
                new EvaluationQuestion { CategoryId = teamLeadCategories[0].Id, QuestionText = "Da li dobro organizuje rad tima?", OrderIndex = 2 },
                new EvaluationQuestion { CategoryId = teamLeadCategories[1].Id, QuestionText = "Da li redovno održava 1-on-1 sastanke?", OrderIndex = 1 },
                new EvaluationQuestion { CategoryId = teamLeadCategories[1].Id, QuestionText = "Da li sluša feedback od tima?", OrderIndex = 2 },
                new EvaluationQuestion { CategoryId = teamLeadCategories[2].Id, QuestionText = "Da li mentoriše junior članove tima?", OrderIndex = 1 },
                new EvaluationQuestion { CategoryId = teamLeadCategories[2].Id, QuestionText = "Da li pomaže u karijernom razvoju?", OrderIndex = 2 }
            });

            await context.EvaluationQuestions.AddRangeAsync(questions);
            await context.SaveChangesAsync();
        }

        private static async Task SeedEvaluationsAsync(ApplicationDbContext context, List<Quarter> quarters, List<User> employees)
        {
            var evaluations = new List<Evaluation>();

            // Za svaki kvartal
            foreach (var quarter in quarters)
            {
                // Uzmi sve Team Lead-ove
                var teamLeads = employees.Where(e => e.SystemRoleId != null &&
                    context.SystemRoles.Any(sr => sr.Id == e.SystemRoleId && sr.Name == "TeamLead")).ToList();

                // Za svakog Team Lead-a, kreiraj evaluacije za sve članove njegovog tima
                foreach (var teamLead in teamLeads)
                {
                    // Pronađi sve članove tima ovog Team Lead-a
                    var teamMembers = employees.Where(e =>
                        e.TeamId == teamLead.TeamId &&
                        e.Id != teamLead.Id).ToList();

                    foreach (var member in teamMembers)
                    {
                        // Team Lead ocenjuje člana tima
                        var questionnaireType = member.RoleId != null ?
                            (context.Roles.First(r => r.Id == member.RoleId).Name switch
                            {
                                "Developer" => QuestionnaireType.DEV,
                                "QA" => QuestionnaireType.QA,
                                "Application Engineer" => QuestionnaireType.APP,
                                _ => QuestionnaireType.DEV
                            }) : QuestionnaireType.DEV;

                        evaluations.Add(new Evaluation
                        {
                            QuarterId = quarter.Id,
                            EvaluatedUserId = member.Id,
                            EvaluatorUserId = teamLead.Id,
                            EvaluationType = EvaluationType.LeadToEmployee,
                            QuestionnaireType = questionnaireType,
                            IsCompleted = false
                        });

                        // Član tima ocenjuje Team Lead-a
                        evaluations.Add(new Evaluation
                        {
                            QuarterId = quarter.Id,
                            EvaluatedUserId = teamLead.Id,
                            EvaluatorUserId = member.Id,
                            EvaluationType = EvaluationType.EmployeeToLead,
                            QuestionnaireType = QuestionnaireType.TeamLead,
                            IsCompleted = false
                        });
                    }
                }
            }

            await context.Evaluations.AddRangeAsync(evaluations);
            await context.SaveChangesAsync();

            Console.WriteLine($"   - {evaluations.Count} evaluations created");
        }
    }
}