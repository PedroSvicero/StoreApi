namespace Core.Entity
{
    public class Usuario : EntityBase
    {
        public required string Email { get; set; }
        public required string SenhaHash { get; set; }

        // Role define o nível de acesso: "Admin" ou "User"
        public required string Role { get; set; }
    }
}