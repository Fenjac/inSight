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

            // 3. KREIRAJ RANKS (23 ranga za svaku od 3 role = 69 total)
            var ranks = new List<Rank>();

            // Praktikant (P1-P3)
            for (int i = 1; i <= 3; i++)
            {
                ranks.Add(new Rank { RoleId = devRole.Id, Code = $"P{i}", Name = $"Praktikant {i}", MinSalary = 30000 + (i * 5000), MaxSalary = 35000 + (i * 5000), MinScore = 0, MaxScore = 0, OrderIndex = i });
                ranks.Add(new Rank { RoleId = qaRole.Id, Code = $"P{i}", Name = $"Praktikant {i}", MinSalary = 30000 + (i * 5000), MaxSalary = 35000 + (i * 5000), MinScore = 0, MaxScore = 0, OrderIndex = i });
                ranks.Add(new Rank { RoleId = appRole.Id, Code = $"P{i}", Name = $"Praktikant {i}", MinSalary = 30000 + (i * 5000), MaxSalary = 35000 + (i * 5000), MinScore = 0, MaxScore = 0, OrderIndex = i });
            }

            // Junior (J1-J6)
            for (int i = 1; i <= 6; i++)
            {
                ranks.Add(new Rank { RoleId = devRole.Id, Code = $"J{i}", Name = $"Junior {i}", MinSalary = 50000 + (i * 10000), MaxSalary = 60000 + (i * 10000), MinScore = 0, MaxScore = 0, OrderIndex = 3 + i });
                ranks.Add(new Rank { RoleId = qaRole.Id, Code = $"J{i}", Name = $"Junior {i}", MinSalary = 50000 + (i * 10000), MaxSalary = 60000 + (i * 10000), MinScore = 0, MaxScore = 0, OrderIndex = 3 + i });
                ranks.Add(new Rank { RoleId = appRole.Id, Code = $"J{i}", Name = $"Junior {i}", MinSalary = 50000 + (i * 10000), MaxSalary = 60000 + (i * 10000), MinScore = 0, MaxScore = 0, OrderIndex = 3 + i });
            }

            // Medior (M1-M6)
            for (int i = 1; i <= 6; i++)
            {
                ranks.Add(new Rank { RoleId = devRole.Id, Code = $"M{i}", Name = $"Medior {i}", MinSalary = 120000 + (i * 15000), MaxSalary = 135000 + (i * 15000), MinScore = 0, MaxScore = 0, OrderIndex = 9 + i });
                ranks.Add(new Rank { RoleId = qaRole.Id, Code = $"M{i}", Name = $"Medior {i}", MinSalary = 120000 + (i * 15000), MaxSalary = 135000 + (i * 15000), MinScore = 0, MaxScore = 0, OrderIndex = 9 + i });
                ranks.Add(new Rank { RoleId = appRole.Id, Code = $"M{i}", Name = $"Medior {i}", MinSalary = 120000 + (i * 15000), MaxSalary = 135000 + (i * 15000), MinScore = 0, MaxScore = 0, OrderIndex = 9 + i });
            }

            // Senior (S1-S6)
            for (int i = 1; i <= 6; i++)
            {
                ranks.Add(new Rank { RoleId = devRole.Id, Code = $"S{i}", Name = $"Senior {i}", MinSalary = 220000 + (i * 20000), MaxSalary = 240000 + (i * 20000), MinScore = 0, MaxScore = 0, OrderIndex = 15 + i });
                ranks.Add(new Rank { RoleId = qaRole.Id, Code = $"S{i}", Name = $"Senior {i}", MinSalary = 220000 + (i * 20000), MaxSalary = 240000 + (i * 20000), MinScore = 0, MaxScore = 0, OrderIndex = 15 + i });
                ranks.Add(new Rank { RoleId = appRole.Id, Code = $"S{i}", Name = $"Senior {i}", MinSalary = 220000 + (i * 20000), MaxSalary = 240000 + (i * 20000), MinScore = 0, MaxScore = 0, OrderIndex = 15 + i });
            }

            // Expert (E1-E3)
            for (int i = 1; i <= 3; i++)
            {
                ranks.Add(new Rank { RoleId = devRole.Id, Code = $"E{i}", Name = $"Expert {i}", MinSalary = 350000 + (i * 30000), MaxSalary = 380000 + (i * 30000), MinScore = 0, MaxScore = 0, OrderIndex = 21 + i });
                ranks.Add(new Rank { RoleId = qaRole.Id, Code = $"E{i}", Name = $"Expert {i}", MinSalary = 350000 + (i * 30000), MaxSalary = 380000 + (i * 30000), MinScore = 0, MaxScore = 0, OrderIndex = 21 + i });
                ranks.Add(new Rank { RoleId = appRole.Id, Code = $"E{i}", Name = $"Expert {i}", MinSalary = 350000 + (i * 30000), MaxSalary = 380000 + (i * 30000), MinScore = 0, MaxScore = 0, OrderIndex = 21 + i });
            }

            await context.Ranks.AddRangeAsync(ranks);
            await context.SaveChangesAsync();

            // Helper function za dobijanje ranga
            Rank GetRank(string code, Guid roleId)
            {
                return ranks.First(r => r.Code == code && r.RoleId == roleId);
            }

            // 4. KREIRAJ TIMOVE
            var teams = new List<Team>
            {
                new Team { Name = "Backend", Description = "Backend development team" },
                new Team { Name = "Backend Boomerang", Description = "Backend boomerang team" },
                new Team { Name = "Frontend", Description = "Frontend development team" },
                new Team { Name = "Frontend Boomerang", Description = "Frontend boomerang team" },
                new Team { Name = "QA", Description = "Quality assurance team" },
                new Team { Name = "Application", Description = "Application engineering team" },
                new Team { Name = "DevOps", Description = "DevOps and infrastructure team" },
                new Team { Name = "AI", Description = "AI and machine learning team" }
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

            // 6. KREIRAJ MANAGEMENT (CEO, CTO, HR)
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

            // 7. KREIRAJ ZAPOSLENE (TEAM LEADS + EMPLOYEES)
            var employees = new List<User>();

            // BACKEND TIM - Lead: Sabo Robert
            employees.Add(new User
            {
                FirstName = "Robert",
                LastName = "Sabo",
                Email = "rsabo@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = teamLeadRole.Id, // TEAM LEAD
                RoleId = devRole.Id,
                CurrentRankId = GetRank("S3", devRole.Id).Id,
                TeamId = backendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Miloš",
                LastName = "Božić",
                Email = "mbozic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M2", devRole.Id).Id,
                TeamId = backendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Kristina",
                LastName = "Vulić",
                Email = "kvulic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("J4", devRole.Id).Id,
                TeamId = backendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Petar",
                LastName = "Lakić",
                Email = "plakic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M3", devRole.Id).Id,
                TeamId = backendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Srđan",
                LastName = "Vidović",
                Email = "svidovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("S1", devRole.Id).Id,
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
                CurrentRankId = GetRank("M4", devRole.Id).Id,
                TeamId = backendBoomerangTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Bogdan",
                LastName = "Marković",
                Email = "bmarkovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("J3", devRole.Id).Id,
                TeamId = backendBoomerangTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Nemanja",
                LastName = "Pavlović",
                Email = "npavlovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M1", devRole.Id).Id,
                TeamId = backendBoomerangTeam.Id,
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
                RoleId = devRole.Id,
                CurrentRankId = GetRank("S2", devRole.Id).Id,
                TeamId = frontendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Miloš",
                LastName = "Antić",
                Email = "mantic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M5", devRole.Id).Id,
                TeamId = frontendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Nikola",
                LastName = "Birovljević",
                Email = "nbirovljevic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("J5", devRole.Id).Id,
                TeamId = frontendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Milan",
                LastName = "Živanović",
                Email = "mzivanovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M2", devRole.Id).Id,
                TeamId = frontendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Stefan",
                LastName = "Gavrilović",
                Email = "sgavrilovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("J6", devRole.Id).Id,
                TeamId = frontendTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Jana",
                LastName = "Knežević",
                Email = "jknezevic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("S1", devRole.Id).Id,
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
                CurrentRankId = GetRank("M6", devRole.Id).Id,
                TeamId = frontendBoomerangTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Aleksa",
                LastName = "Drašković",
                Email = "adraskovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("J2", devRole.Id).Id,
                TeamId = frontendBoomerangTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Mihailo",
                LastName = "Gaćeša",
                Email = "mgacesa@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M3", devRole.Id).Id,
                TeamId = frontendBoomerangTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Ranko",
                LastName = "Glišović",
                Email = "rglisovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("J1", devRole.Id).Id,
                TeamId = frontendBoomerangTeam.Id,
                IsActive = true
            });

            // DEVOPS TIM - Lead: Apatović Aleksandar
            employees.Add(new User
            {
                FirstName = "Aleksandar",
                LastName = "Apatović",
                Email = "aapatovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = teamLeadRole.Id, // TEAM LEAD
                RoleId = devRole.Id,
                CurrentRankId = GetRank("S4", devRole.Id).Id,
                TeamId = devopsTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Lazar",
                LastName = "Ranković",
                Email = "lrankovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M4", devRole.Id).Id,
                TeamId = devopsTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Zoran",
                LastName = "Pavkov",
                Email = "zpavkov@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("S1", devRole.Id).Id,
                TeamId = devopsTeam.Id,
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
                CurrentRankId = GetRank("S5", devRole.Id).Id,
                TeamId = aiTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Nikola",
                LastName = "Jovanović",
                Email = "njovanovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("M6", devRole.Id).Id,
                TeamId = aiTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Marija",
                LastName = "Stanković",
                Email = "mstankovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = devRole.Id,
                CurrentRankId = GetRank("J4", devRole.Id).Id,
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
                CurrentRankId = GetRank("M4", qaRole.Id).Id,
                TeamId = qaTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Ana",
                LastName = "Petrović",
                Email = "apetrovic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = qaRole.Id,
                CurrentRankId = GetRank("J3", qaRole.Id).Id,
                TeamId = qaTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Igor",
                LastName = "Nikolić",
                Email = "inikolic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = qaRole.Id,
                CurrentRankId = GetRank("M2", qaRole.Id).Id,
                TeamId = qaTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Jovana",
                LastName = "Marić",
                Email = "jmaric@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = qaRole.Id,
                CurrentRankId = GetRank("J1", qaRole.Id).Id,
                TeamId = qaTeam.Id,
                IsActive = true
            });

            employees.Add(new User
            {
                FirstName = "Stefan",
                LastName = "Đorđević",
                Email = "sdjordjevic@industrial-it.software",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                SystemRoleId = employeeRole.Id,
                RoleId = qaRole.Id,
                CurrentRankId = GetRank("P1", qaRole.Id).Id,
                TeamId = frontendBoomerangTeam.Id, // Neki QA su u drugim timovima
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

            // 10. KREIRAJ EVALUACIJE SA ODGOVORIMA za oba kvartala
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
            Console.WriteLine("📝 Kreiram evaluacije sa odgovorima...");

            var q3 = quarters.First(q => q.QuarterNumber == 3 && q.Year == 2024);
            var q4 = quarters.First(q => q.QuarterNumber == 4 && q.Year == 2024);

            // Dohvati sve pitanja po tipu upitnika
            var devQuestions = await context.EvaluationQuestions
                .Include(q => q.Category)
                .Where(q => q.Category.QuestionnaireType == QuestionnaireType.DEV)
                .ToListAsync();

            var qaQuestions = await context.EvaluationQuestions
                .Include(q => q.Category)
                .Where(q => q.Category.QuestionnaireType == QuestionnaireType.QA)
                .ToListAsync();

            var appQuestions = await context.EvaluationQuestions
                .Include(q => q.Category)
                .Where(q => q.Category.QuestionnaireType == QuestionnaireType.APP)
                .ToListAsync();

            var teamLeadQuestions = await context.EvaluationQuestions
                .Include(q => q.Category)
                .Where(q => q.Category.QuestionnaireType == QuestionnaireType.TeamLead)
                .ToListAsync();

            var random = new Random(42); // Fixed seed za konzistentne rezultate

            // Dohvati Team Leads i Employees
            var teamLeads = employees.Where(e => e.SystemRole?.Name == "TeamLead").ToList();
            var regularEmployees = employees.Where(e => e.SystemRole?.Name == "Employee").ToList();

            var evaluations = new List<Evaluation>();
            var allAnswers = new List<EvaluationAnswer>();

            // =====================================================
            // Q3 2024 - POTPUNO ZAVRŠEN KVARTAL (100% completed)
            // =====================================================

            Console.WriteLine($"   Kreiram evaluacije za Q3 2024 (potpuno završen)...");

            foreach (var employee in regularEmployees)
            {
                // Pronađi Team Lead-a za ovog zaposlenog
                var teamLead = teamLeads.FirstOrDefault(tl => tl.TeamId == employee.TeamId);

                if (teamLead != null)
                {
                    // 1. LEAD → EMPLOYEE evaluacija (ZAVRŠENA)
                    var leadToEmpEval = new Evaluation
                    {
                        Id = Guid.NewGuid(),
                        QuarterId = q3.Id,
                        EvaluatedUserId = employee.Id,
                        EvaluatorUserId = teamLead.Id,
                        EvaluationType = EvaluationType.LeadToEmployee,
                        QuestionnaireType = GetQuestionnaireTypeForRole(employee.Role?.Name),
                        IsCompleted = true,
                        CompletedAt = q3.EndDate.AddDays(-random.Next(1, 15)), // Završeno unutar kvartala
                        CreatedAt = q3.StartDate.AddDays(1),
                        UpdatedAt = q3.EndDate.AddDays(-random.Next(1, 15))
                    };

                    evaluations.Add(leadToEmpEval);

                    // Dodaj odgovore za ovu evaluaciju
                    var questions = GetQuestionsForType(leadToEmpEval.QuestionnaireType,
                        devQuestions, qaQuestions, appQuestions, teamLeadQuestions);

                    var answers = CreateAnswersForEvaluation(leadToEmpEval.Id, questions, random);
                    allAnswers.AddRange(answers);

                    // Postavi OverallScore kao prosek
                    leadToEmpEval.OverallScore = (decimal)answers.Average(a => a.Score);

                    // 2. EMPLOYEE → LEAD evaluacija (ZAVRŠENA)
                    var empToLeadEval = new Evaluation
                    {
                        Id = Guid.NewGuid(),
                        QuarterId = q3.Id,
                        EvaluatedUserId = teamLead.Id,
                        EvaluatorUserId = employee.Id,
                        EvaluationType = EvaluationType.EmployeeToLead,
                        QuestionnaireType = QuestionnaireType.TeamLead,
                        IsCompleted = true,
                        CompletedAt = q3.EndDate.AddDays(-random.Next(1, 15)),
                        CreatedAt = q3.StartDate.AddDays(1),
                        UpdatedAt = q3.EndDate.AddDays(-random.Next(1, 15))
                    };

                    evaluations.Add(empToLeadEval);

                    var leadAnswers = CreateAnswersForEvaluation(empToLeadEval.Id, teamLeadQuestions, random);
                    allAnswers.AddRange(leadAnswers);
                    empToLeadEval.OverallScore = (decimal)leadAnswers.Average(a => a.Score);
                }
            }

            // =====================================================
            // Q4 2024 - AKTIVAN KVARTAL (50% completed, 50% in progress)
            // =====================================================

            Console.WriteLine($"   Kreiram evaluacije za Q4 2024 (aktivan, delimično završen)...");

            int completedCount = 0;
            int totalQ4 = regularEmployees.Count * 2; // Lead→Emp + Emp→Lead

            foreach (var employee in regularEmployees)
            {
                var teamLead = teamLeads.FirstOrDefault(tl => tl.TeamId == employee.TeamId);

                if (teamLead != null)
                {
                    bool shouldComplete = completedCount < totalQ4 / 2; // 50% će biti završeno

                    // 1. LEAD → EMPLOYEE evaluacija
                    var leadToEmpEval = new Evaluation
                    {
                        Id = Guid.NewGuid(),
                        QuarterId = q4.Id,
                        EvaluatedUserId = employee.Id,
                        EvaluatorUserId = teamLead.Id,
                        EvaluationType = EvaluationType.LeadToEmployee,
                        QuestionnaireType = GetQuestionnaireTypeForRole(employee.Role?.Name),
                        IsCompleted = shouldComplete,
                        CreatedAt = q4.StartDate.AddDays(1)
                    };

                    if (shouldComplete)
                    {
                        leadToEmpEval.CompletedAt = DateTime.UtcNow.AddDays(-random.Next(1, 7));
                        leadToEmpEval.UpdatedAt = leadToEmpEval.CompletedAt;

                        var questions = GetQuestionsForType(leadToEmpEval.QuestionnaireType,
                            devQuestions, qaQuestions, appQuestions, teamLeadQuestions);
                        var answers = CreateAnswersForEvaluation(leadToEmpEval.Id, questions, random);
                        allAnswers.AddRange(answers);
                        leadToEmpEval.OverallScore = (decimal)answers.Average(a => a.Score);
                        completedCount++;
                    }

                    evaluations.Add(leadToEmpEval);

                    // 2. EMPLOYEE → LEAD evaluacija
                    shouldComplete = completedCount < totalQ4 / 2;

                    var empToLeadEval = new Evaluation
                    {
                        Id = Guid.NewGuid(),
                        QuarterId = q4.Id,
                        EvaluatedUserId = teamLead.Id,
                        EvaluatorUserId = employee.Id,
                        EvaluationType = EvaluationType.EmployeeToLead,
                        QuestionnaireType = QuestionnaireType.TeamLead,
                        IsCompleted = shouldComplete,
                        CreatedAt = q4.StartDate.AddDays(1)
                    };

                    if (shouldComplete)
                    {
                        empToLeadEval.CompletedAt = DateTime.UtcNow.AddDays(-random.Next(1, 7));
                        empToLeadEval.UpdatedAt = empToLeadEval.CompletedAt;

                        var leadAnswers = CreateAnswersForEvaluation(empToLeadEval.Id, teamLeadQuestions, random);
                        allAnswers.AddRange(leadAnswers);
                        empToLeadEval.OverallScore = (decimal)leadAnswers.Average(a => a.Score);
                        completedCount++;
                    }

                    evaluations.Add(empToLeadEval);
                }
            }

            await context.Evaluations.AddRangeAsync(evaluations);
            await context.EvaluationAnswers.AddRangeAsync(allAnswers);
            await context.SaveChangesAsync();

            Console.WriteLine($"   ✅ Kreirano {evaluations.Count} evaluacija:");
            Console.WriteLine($"      - Q3 2024: {evaluations.Count(e => e.QuarterId == q3.Id)} evaluacija (100% završeno)");
            Console.WriteLine($"      - Q4 2024: {evaluations.Count(e => e.QuarterId == q4.Id)} evaluacija (~50% završeno)");
            Console.WriteLine($"   ✅ Kreirano {allAnswers.Count} odgovora na pitanja");
        }

        private static QuestionnaireType GetQuestionnaireTypeForRole(string? roleName)
        {
            return roleName switch
            {
                "Developer" => QuestionnaireType.DEV,
                "QA" => QuestionnaireType.QA,
                "Application Engineer" => QuestionnaireType.APP,
                _ => QuestionnaireType.DEV
            };
        }

        private static List<EvaluationQuestion> GetQuestionsForType(
            QuestionnaireType type,
            List<EvaluationQuestion> devQ,
            List<EvaluationQuestion> qaQ,
            List<EvaluationQuestion> appQ,
            List<EvaluationQuestion> leadQ)
        {
            return type switch
            {
                QuestionnaireType.DEV => devQ,
                QuestionnaireType.QA => qaQ,
                QuestionnaireType.APP => appQ,
                QuestionnaireType.TeamLead => leadQ,
                _ => devQ
            };
        }

        private static List<EvaluationAnswer> CreateAnswersForEvaluation(
            Guid evaluationId,
            List<EvaluationQuestion> questions,
            Random random)
        {
            var answers = new List<EvaluationAnswer>();

            foreach (var question in questions)
            {
                // Generiši realistične ocene sa težinom ka višim ocenama (3-5)
                int score = GenerateRealisticScore(random);

                var answer = new EvaluationAnswer
                {
                    Id = Guid.NewGuid(),
                    EvaluationId = evaluationId,
                    QuestionId = question.Id,
                    Score = score,
                    Comment = score != 3 ? GenerateComment(score, random) : null, // Komentar samo ako nije 3
                    CreatedAt = DateTime.UtcNow
                };

                answers.Add(answer);
            }

            return answers;
        }

        private static int GenerateRealisticScore(Random random)
        {
            // Težine za ocene: većina 3-4, nešto 5, retko 1-2
            var distribution = new[] { 1, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5 };
            return distribution[random.Next(distribution.Length)];
        }

        private static string? GenerateComment(int score, Random random)
        {
            var positiveComments = new[]
            {
                "Odličan rad, nastaviti u istom pravcu.",
                "Pokazuje izuzetne veštine i posvećenost.",
                "Vrlo dobar učinak, mali prostor za poboljšanje.",
                "Konstantno ispunjava i premašuje očekivanja.",
                "Odlična timska saradnja."
            };

            var negativeComments = new[]
            {
                "Potrebno je više fokusa na detalje.",
                "Postoji prostor za poboljšanje u ovoj oblasti.",
                "Potrebna dodatna obuka ili mentorstvo.",
                "Treba raditi na boljoj komunikaciji sa timom.",
                "Očekujemo veći napredak u narednom periodu."
            };

            return score switch
            {
                5 => positiveComments[random.Next(positiveComments.Length)],
                4 => positiveComments[random.Next(positiveComments.Length)],
                3 => null, // Neutralno, bez komentara
                2 => negativeComments[random.Next(negativeComments.Length)],
                1 => negativeComments[random.Next(negativeComments.Length)],
                _ => null
            };
        }
    }
}