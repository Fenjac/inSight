using inSight.API.Models;
using Microsoft.EntityFrameworkCore;

namespace inSight.API.Data
{
    /// <summary>
    /// Production Database Seeder - Seeds only lookup tables (SystemRoles, Roles, Teams, Ranks)
    /// Does NOT seed test users or test data
    /// </summary>
    public static class DbSeederProduction
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            Console.WriteLine("🌱 DbSeederProduction.SeedAsync STARTED!");

            await SeedSystemRolesAsync(context);
            await SeedJobRolesAsync(context);
            await SeedTeamsAsync(context);
            await SeedRanksAsync(context);
            await SeedQuartersAsync(context);
            await SeedRankThresholdsAsync(context);
            await SeedUsersFromCsvAsync(context);
            await SeedQuarterlyScoresFromCsvAsync(context);

            Console.WriteLine("✅ Production seed completed successfully!");
        }

        private static async Task SeedSystemRolesAsync(ApplicationDbContext context)
        {
            if (await context.SystemRoles.AnyAsync())
            {
                Console.WriteLine("⚠️ SystemRoles already exist, skipping...");
                return;
            }

            var systemRoles = new List<SystemRole>
            {
                new SystemRole { Name = "Engineer", Description = "Team member / Engineer", PermissionLevel = 1 },
                new SystemRole { Name = "TeamLead", Description = "Team Leader", PermissionLevel = 2 },
                new SystemRole { Name = "HeadOfPeople", Description = "Head of People and Organizational Management", PermissionLevel = 3 },
                new SystemRole { Name = "CTO", Description = "Chief Technology Officer", PermissionLevel = 4 },
                new SystemRole { Name = "CEO", Description = "Chief Executive Officer", PermissionLevel = 5 }
            };

            await context.SystemRoles.AddRangeAsync(systemRoles);
            await context.SaveChangesAsync();

            Console.WriteLine($"   ✅ Created {systemRoles.Count} System Roles");
        }

        private static async Task SeedJobRolesAsync(ApplicationDbContext context)
        {
            if (await context.Roles.AnyAsync())
            {
                Console.WriteLine("⚠️ Job Roles already exist, skipping...");
                return;
            }

            var jobRoles = new List<Role>
            {
                new Role { Name = "Developer", Description = "Software Developer" },
                new Role { Name = "QA Engineer", Description = "Quality Assurance Engineer" },
                new Role { Name = "Application Engineer", Description = "Mobile/Application Engineer" }
            };

            await context.Roles.AddRangeAsync(jobRoles);
            await context.SaveChangesAsync();

            Console.WriteLine($"   ✅ Created {jobRoles.Count} Job Roles");
        }

        private static async Task SeedTeamsAsync(ApplicationDbContext context)
        {
            if (await context.Teams.AnyAsync())
            {
                Console.WriteLine("⚠️ Teams already exist, skipping...");
                return;
            }

            var teams = new List<Team>
            {
                new Team { Name = "Backend", Description = "Backend Development Team" },
                new Team { Name = "Backend Boomerang", Description = "Backend Boomerang Team" },
                new Team { Name = "Frontend", Description = "Frontend Development Team" },
                new Team { Name = "Frontend Boomerang", Description = "Frontend Boomerang Team" },
                new Team { Name = "QA", Description = "Quality Assurance Team" },
                new Team { Name = "Application", Description = "Mobile/Application Engineering Team" },
                new Team { Name = "DevOps", Description = "DevOps and Infrastructure Team" },
                new Team { Name = "AI", Description = "AI and Machine Learning Team" }
            };

            await context.Teams.AddRangeAsync(teams);
            await context.SaveChangesAsync();

            Console.WriteLine($"   ✅ Created {teams.Count} Teams");
        }

        private static async Task SeedRanksAsync(ApplicationDbContext context)
        {
            if (await context.Ranks.AnyAsync())
            {
                Console.WriteLine("⚠️ Ranks already exist, skipping...");
                return;
            }

            var ranks = new List<Rank>();

            // Helper method to create a single rank (not role-specific)
            void AddRank(string code, string name, int minSalary, int maxSalary, int orderIndex)
            {
                ranks.Add(new Rank
                {
                    Code = code,
                    Name = name,
                    MinSalary = minSalary,
                    MaxSalary = maxSalary,
                    OrderIndex = orderIndex
                });
            }

            // Praktikant (P1-P3)
            AddRank("P1", "Praktikant 1", 30000, 35000, 1);
            AddRank("P2", "Praktikant 2", 35000, 40000, 2);
            AddRank("P3", "Praktikant 3", 40000, 45000, 3);

            // Junior (J1-J6)
            AddRank("J1", "Junior 1", 50000, 60000, 4);
            AddRank("J2", "Junior 2", 60000, 70000, 5);
            AddRank("J3", "Junior 3", 70000, 80000, 6);
            AddRank("J4", "Junior 4", 80000, 90000, 7);
            AddRank("J5", "Junior 5", 90000, 100000, 8);
            AddRank("J6", "Junior 6", 100000, 110000, 9);

            // Medior (M1-M6)
            AddRank("M1", "Medior 1", 120000, 135000, 10);
            AddRank("M2", "Medior 2", 135000, 150000, 11);
            AddRank("M3", "Medior 3", 150000, 165000, 12);
            AddRank("M4", "Medior 4", 165000, 180000, 13);
            AddRank("M5", "Medior 5", 180000, 195000, 14);
            AddRank("M6", "Medior 6", 195000, 210000, 15);

            // Senior (S1-S6)
            AddRank("S1", "Senior 1", 220000, 240000, 16);
            AddRank("S2", "Senior 2", 240000, 260000, 17);
            AddRank("S3", "Senior 3", 260000, 280000, 18);
            AddRank("S4", "Senior 4", 280000, 300000, 19);
            AddRank("S5", "Senior 5", 300000, 320000, 20);
            AddRank("S6", "Senior 6", 320000, 340000, 21);

            // Expert (E1-E3)
            AddRank("E1", "Expert 1", 350000, 380000, 22);
            AddRank("E2", "Expert 2", 380000, 410000, 23);
            AddRank("E3", "Expert 3", 410000, 440000, 24);

            await context.Ranks.AddRangeAsync(ranks);
            await context.SaveChangesAsync();

            Console.WriteLine($"   ✅ Created {ranks.Count} Ranks (universal for all job roles)");
        }

        private static async Task SeedQuartersAsync(ApplicationDbContext context)
        {
            if (await context.Quarters.AnyAsync())
            {
                Console.WriteLine("⚠️ Quarters already exist, skipping...");
                return;
            }

            var quarters = new List<Quarter>();

            // Seed quarters from Q2 2024 to Q3 2025 (based on CSV data)
            // Q2 2024
            quarters.Add(new Quarter { Year = 2024, QuarterNumber = 2, StartDate = new DateTime(2024, 4, 1), EndDate = new DateTime(2024, 6, 30) });
            // Q3 2024
            quarters.Add(new Quarter { Year = 2024, QuarterNumber = 3, StartDate = new DateTime(2024, 7, 1), EndDate = new DateTime(2024, 9, 30) });
            // Q4 2024
            quarters.Add(new Quarter { Year = 2024, QuarterNumber = 4, StartDate = new DateTime(2024, 10, 1), EndDate = new DateTime(2024, 12, 31) });
            // Q1 2025
            quarters.Add(new Quarter { Year = 2025, QuarterNumber = 1, StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 3, 31) });
            // Q2 2025
            quarters.Add(new Quarter { Year = 2025, QuarterNumber = 2, StartDate = new DateTime(2025, 4, 1), EndDate = new DateTime(2025, 6, 30) });
            // Q3 2025
            quarters.Add(new Quarter { Year = 2025, QuarterNumber = 3, StartDate = new DateTime(2025, 7, 1), EndDate = new DateTime(2025, 9, 30) });

            await context.Quarters.AddRangeAsync(quarters);
            await context.SaveChangesAsync();

            Console.WriteLine($"   ✅ Created {quarters.Count} Quarters");
        }

        private static async Task SeedUsersFromCsvAsync(ApplicationDbContext context)
        {
            if (await context.Users.AnyAsync())
            {
                Console.WriteLine("⚠️ Users already exist, skipping CSV import...");
                return;
            }

            var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Data", "initemp.csv");

            if (!File.Exists(csvPath))
            {
                Console.WriteLine($"⚠️ CSV file not found at: {csvPath}");
                return;
            }

            Console.WriteLine($"📊 Reading CSV file from: {csvPath}");

            // Load lookup data
            var systemRoles = await context.SystemRoles.ToListAsync();
            var jobRoles = await context.Roles.ToListAsync();
            var teams = await context.Teams.ToListAsync();
            var ranks = await context.Ranks.ToListAsync();

            var users = new List<User>();
            var userCounter = 0;
            var lineNumber = 0;

            using (var reader = new StreamReader(csvPath))
            {
                // Skip header line
                var header = await reader.ReadLineAsync();
                lineNumber++;

                Console.WriteLine($"   📋 CSV Header: {header}");

                while (!reader.EndOfStream)
                {
                    lineNumber++;
                    var line = await reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    try
                    {
                        // Split CSV line by comma
                        var columns = line.Split(',');

                        if (columns.Length < 6)
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: Invalid format, expected 6 columns but got {columns.Length}");
                            continue;
                        }

                        // Column 0: Prezime Ime
                        var fullName = columns[0].Trim();
                        if (string.IsNullOrWhiteSpace(fullName))
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: Empty name, skipping...");
                            continue;
                        }

                        // Split name (format: "Prezime Ime")
                        var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (nameParts.Length < 2)
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: Invalid name format '{fullName}', skipping...");
                            continue;
                        }

                        var lastName = nameParts[0];
                        var firstName = string.Join(" ", nameParts.Skip(1));

                        // Column 1: Rang
                        var rankCode = columns[1].Trim().ToUpper();

                        // Column 2: Email
                        var email = columns[2].Trim().ToLower();
                        if (string.IsNullOrWhiteSpace(email))
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: No email for {fullName}, skipping...");
                            continue;
                        }

                        // Column 3: Prezime Ime Nadredjenog (not used)

                        // Column 4: Tim
                        var teamName = columns[4].Trim();

                        // Column 5: Da li je lider
                        var isLeaderValue = columns[5].Trim().ToLower();
                        var isLeader = isLeaderValue == "da" || isLeaderValue == "yes" || isLeaderValue == "1" || isLeaderValue == "true";

                        // Determine SystemRole based on leadership
                        var systemRole = isLeader
                            ? systemRoles.FirstOrDefault(sr => sr.Name == "TeamLead")
                            : systemRoles.FirstOrDefault(sr => sr.Name == "Engineer");

                        if (systemRole == null)
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: SystemRole not found, skipping...");
                            continue;
                        }

                        // Find Team
                        var team = teams.FirstOrDefault(t => t.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
                        if (team == null && !string.IsNullOrWhiteSpace(teamName))
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: Team '{teamName}' not found for {fullName}");
                        }

                        // Determine JobRole based on Team
                        Role? jobRole = null;
                        if (team != null)
                        {
                            if (team.Name.Equals("QA", StringComparison.OrdinalIgnoreCase))
                            {
                                jobRole = jobRoles.FirstOrDefault(r => r.Name == "QA Engineer");
                            }
                            else if (team.Name.Equals("Application", StringComparison.OrdinalIgnoreCase))
                            {
                                jobRole = jobRoles.FirstOrDefault(r => r.Name == "Application Engineer");
                            }
                            else
                            {
                                jobRole = jobRoles.FirstOrDefault(r => r.Name == "Developer");
                            }
                        }
                        else
                        {
                            // Default to Developer if no team
                            jobRole = jobRoles.FirstOrDefault(r => r.Name == "Developer");
                        }

                        if (jobRole == null)
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: JobRole not found, skipping...");
                            continue;
                        }

                        // Find Rank based on RankCode and JobRole
                        Rank? rank = null;
                        if (!string.IsNullOrWhiteSpace(rankCode))
                        {
                            rank = ranks.FirstOrDefault(r =>
                                r.Code.Equals(rankCode, StringComparison.OrdinalIgnoreCase));

                            if (rank == null)
                            {
                                Console.WriteLine($"   ⚠️ Line {lineNumber}: Rank '{rankCode}' not found for {fullName}");
                            }
                        }

                        // Create User
                        var user = new User
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            Email = email,
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                            SystemRoleId = systemRole.Id,
                            RoleId = jobRole.Id,
                            CurrentRankId = rank?.Id,
                            TeamId = team?.Id,
                            IsActive = true
                        };

                        users.Add(user);
                        userCounter++;

                        if (userCounter % 10 == 0)
                        {
                            Console.WriteLine($"   📝 Processed {userCounter} users...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"   ❌ Error processing line {lineNumber}: {ex.Message}");
                    }
                }
            }

            if (users.Count > 0)
            {
                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
                Console.WriteLine($"   ✅ Created {users.Count} users from CSV");
            }
            else
            {
                Console.WriteLine("   ⚠️ No users were created from CSV");
            }
        }

        private static async Task SeedRankThresholdsAsync(ApplicationDbContext context)
        {
            if (await context.RankScoreThresholds.AnyAsync())
            {
                Console.WriteLine("⚠️ RankScoreThresholds already exist, skipping...");
                return;
            }

            var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Data", "ranks.csv");

            if (!File.Exists(csvPath))
            {
                Console.WriteLine($"⚠️ ranks.csv file not found at: {csvPath}");
                return;
            }

            Console.WriteLine($"📊 Reading ranks.csv file from: {csvPath}");

            // Load lookup data
            var ranks = await context.Ranks.ToListAsync();
            var roles = await context.Roles.ToListAsync();

            var thresholds = new List<RankScoreThreshold>();
            var lineNumber = 0;

            using (var reader = new StreamReader(csvPath))
            {
                // Skip header line
                var header = await reader.ReadLineAsync();
                lineNumber++;

                Console.WriteLine($"   📋 CSV Header: {header}");

                while (!reader.EndOfStream)
                {
                    lineNumber++;
                    var line = await reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    try
                    {
                        // Split CSV line by comma
                        var columns = line.Split(',');

                        if (columns.Length < 3)
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: Invalid format, expected 3 columns but got {columns.Length}");
                            continue;
                        }

                        // Column 0: RANG (e.g., P1, P2, J1, etc.)
                        var rankCode = columns[0].Trim().ToUpper();

                        // Column 1: PRAG (MinScore)
                        var minScoreStr = columns[1].Trim();
                        if (!int.TryParse(minScoreStr, out int minScore))
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: Invalid MinScore '{minScoreStr}' for rank '{rankCode}'");
                            continue;
                        }

                        // Column 2: MAX (MaxScore)
                        var maxScoreStr = columns[2].Trim();
                        if (!int.TryParse(maxScoreStr, out int maxScore))
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: Invalid MaxScore '{maxScoreStr}' for rank '{rankCode}'");
                            continue;
                        }

                        // Find the rank with this code (now there's only one per code)
                        var rank = ranks.FirstOrDefault(r => r.Code.Equals(rankCode, StringComparison.OrdinalIgnoreCase));

                        if (rank == null)
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: No rank found with code '{rankCode}'");
                            continue;
                        }

                        // Create threshold for each role (Developer, QA Engineer, Application Engineer)
                        foreach (var role in roles)
                        {
                            thresholds.Add(new RankScoreThreshold
                            {
                                RankId = rank.Id,
                                RoleId = role.Id,
                                MinScore = minScore,
                                MaxScore = maxScore,
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"   ❌ Error processing line {lineNumber}: {ex.Message}");
                    }
                }
            }

            if (thresholds.Count > 0)
            {
                await context.RankScoreThresholds.AddRangeAsync(thresholds);
                await context.SaveChangesAsync();
                Console.WriteLine($"   ✅ Created {thresholds.Count} rank score thresholds from CSV");
            }
            else
            {
                Console.WriteLine("   ⚠️ No rank score thresholds were created from CSV");
            }
        }

        private static async Task SeedQuarterlyScoresFromCsvAsync(ApplicationDbContext context)
        {
            if (await context.QuarterlyScores.AnyAsync())
            {
                Console.WriteLine("⚠️ QuarterlyScores already exist, skipping...");
                return;
            }

            var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Data", "initemp.csv");

            if (!File.Exists(csvPath))
            {
                Console.WriteLine($"⚠️ CSV file not found at: {csvPath}");
                return;
            }

            Console.WriteLine($"📊 Reading quarterly scores from CSV: {csvPath}");

            // Load lookup data
            var users = await context.Users.ToListAsync();
            var quarters = await context.Quarters.ToListAsync();

            // Map quarters by year and quarter number for easy lookup
            var quarterMap = quarters.ToDictionary(q => $"Q{q.QuarterNumber} {q.Year}", q => q);

            var quarterlyScores = new List<QuarterlyScore>();
            var lineNumber = 0;
            var scoreCounter = 0;

            using (var reader = new StreamReader(csvPath))
            {
                // Read header line
                var header = await reader.ReadLineAsync();
                lineNumber++;

                if (string.IsNullOrWhiteSpace(header))
                {
                    Console.WriteLine("   ⚠️ CSV header is empty");
                    return;
                }

                // Parse header to find column indices
                var columns = header.Split(',');
                var inicialnoIndex = Array.FindIndex(columns, c => c.Trim().Equals("inicijalno", StringComparison.OrdinalIgnoreCase));

                // Map quarter columns: "Q2 2024", "Q2 2024 bonus", etc.
                var quarterScoreColumns = new Dictionary<string, int>(); // Quarter key -> column index
                var quarterBonusColumns = new Dictionary<string, int>(); // Quarter key -> bonus column index

                for (int i = 0; i < columns.Length; i++)
                {
                    var col = columns[i].Trim();
                    if (col.StartsWith("Q", StringComparison.OrdinalIgnoreCase))
                    {
                        if (col.Contains("bonus", StringComparison.OrdinalIgnoreCase))
                        {
                            // Extract quarter key (e.g., "Q2 2024 bonus" -> "Q2 2024")
                            var quarterKey = col.Replace("bonus", "", StringComparison.OrdinalIgnoreCase).Trim();
                            quarterBonusColumns[quarterKey] = i;
                        }
                        else
                        {
                            // Regular quarter score column
                            quarterScoreColumns[col] = i;
                        }
                    }
                }

                Console.WriteLine($"   📋 Found {inicialnoIndex >= 0} inicijalno column at index {inicialnoIndex}");
                Console.WriteLine($"   📋 Found {quarterScoreColumns.Count} quarter score columns");
                Console.WriteLine($"   📋 Found {quarterBonusColumns.Count} quarter bonus columns");

                while (!reader.EndOfStream)
                {
                    lineNumber++;
                    var line = await reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    try
                    {
                        var cols = line.Split(',');

                        // Column 2: Email (for finding the user)
                        if (cols.Length < 3)
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: Not enough columns");
                            continue;
                        }

                        var email = cols[2].Trim().ToLower();
                        var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                        if (user == null)
                        {
                            Console.WriteLine($"   ⚠️ Line {lineNumber}: User not found for email '{email}'");
                            continue;
                        }

                        // Get inicijalno (starting score)
                        if (inicialnoIndex >= 0 && inicialnoIndex < cols.Length)
                        {
                            var inicialnoStr = cols[inicialnoIndex].Trim();
                            if (int.TryParse(inicialnoStr, out int inicijalno))
                            {
                                user.StartingScore = inicijalno;
                            }
                        }

                        // Process each quarter
                        foreach (var quarterEntry in quarterScoreColumns)
                        {
                            var quarterKey = quarterEntry.Key; // e.g., "Q2 2024"
                            var scoreColIndex = quarterEntry.Value;

                            if (scoreColIndex >= cols.Length)
                            {
                                continue;
                            }

                            var scoreStr = cols[scoreColIndex].Trim();

                            // Skip if score is "-" or empty
                            if (string.IsNullOrWhiteSpace(scoreStr) || scoreStr == "-")
                            {
                                continue;
                            }

                            if (!int.TryParse(scoreStr, out int baseScore))
                            {
                                Console.WriteLine($"   ⚠️ Line {lineNumber}: Invalid score '{scoreStr}' for {quarterKey}");
                                continue;
                            }

                            // Get bonus if exists
                            int bonusScore = 0;
                            if (quarterBonusColumns.TryGetValue(quarterKey, out int bonusColIndex) && bonusColIndex < cols.Length)
                            {
                                var bonusStr = cols[bonusColIndex].Trim();
                                if (!string.IsNullOrWhiteSpace(bonusStr) && bonusStr != "-")
                                {
                                    if (int.TryParse(bonusStr, out int parsedBonus))
                                    {
                                        bonusScore = parsedBonus;
                                    }
                                }
                            }

                            // Find the quarter
                            if (!quarterMap.TryGetValue(quarterKey, out var quarter))
                            {
                                Console.WriteLine($"   ⚠️ Line {lineNumber}: Quarter '{quarterKey}' not found in database");
                                continue;
                            }

                            // Create QuarterlyScore
                            var quarterlyScore = new QuarterlyScore
                            {
                                UserId = user.Id,
                                QuarterId = quarter.Id,
                                BaseScore = baseScore,
                                ManagementBonusScore = bonusScore,
                                ManagementBonusReason = bonusScore > 0 ? "Imported from CSV" : null,
                                LeaderAverageScore = null, // Will be calculated later
                                TotalScore = baseScore + bonusScore,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };

                            quarterlyScores.Add(quarterlyScore);
                            scoreCounter++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"   ❌ Error processing line {lineNumber}: {ex.Message}");
                    }
                }
            }

            // Save users with updated StartingScore
            await context.SaveChangesAsync();
            Console.WriteLine($"   ✅ Updated StartingScore for {users.Count(u => u.StartingScore > 0)} users");

            // Calculate CurrentTotalScore for each user
            foreach (var user in users)
            {
                var userScores = quarterlyScores.Where(qs => qs.UserId == user.Id).ToList();
                user.CurrentTotalScore = user.StartingScore + userScores.Sum(qs => qs.TotalScore);
            }

            await context.SaveChangesAsync();
            Console.WriteLine($"   ✅ Calculated CurrentTotalScore for {users.Count} users");

            if (quarterlyScores.Count > 0)
            {
                await context.QuarterlyScores.AddRangeAsync(quarterlyScores);
                await context.SaveChangesAsync();
                Console.WriteLine($"   ✅ Created {quarterlyScores.Count} quarterly scores from CSV");
            }
            else
            {
                Console.WriteLine("   ⚠️ No quarterly scores were created from CSV");
            }
        }
    }
}
