namespace inSight.API.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string? CurrentRank { get; set; }
        public int StartingScore { get; set; }
        public int CurrentTotalScore { get; set; }
        public bool IsActive { get; set; }
        public Guid? TeamLeadId { get; set; }
        public string? TeamLeadName { get; set; }
        public DateTime CreatedAt { get; set; }

        // IDs needed for edit operations
        public Guid? SystemRoleId { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? CurrentRankId { get; set; }
        public Guid? TeamId { get; set; }

        // Nested objects for display purposes
        public SystemRoleDto? SystemRoleDetails { get; set; }
        public RoleDto? RoleDetails { get; set; }
        public RankDto? CurrentRankDetails { get; set; }
        public string? TeamName { get; set; }
    }

    public class SystemRoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class RankDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class UserWithTeamDto : UserDto
    {
        public List<UserDto> TeamMembers { get; set; } = new();
    }

    public class CreateUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SystemRoleId { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public string? CurrentRankId { get; set; }
        public string? TeamId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string SystemRoleId { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public string? CurrentRankId { get; set; }
        public string? TeamId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}