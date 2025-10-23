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
        public bool IsActive { get; set; }
        public Guid? TeamLeadId { get; set; }
        public string? TeamLeadName { get; set; }
    }

    public class UserWithTeamDto : UserDto
    {
        public List<UserDto> TeamMembers { get; set; } = new();
    }
}