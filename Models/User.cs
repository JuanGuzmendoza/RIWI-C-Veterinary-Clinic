using System;

namespace VeterinaryClinic.Models
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Name { get; set; }         // Nombre del usuario
        public string Username { get; set; }     // Usuario de login
        public string Password { get; set; }     // Contraseña
        public string Role { get; set; }         // Rol (Admin, Veterinarian, Customer, etc.)
        public Guid EntityId { get; set; }       // ID de la persona o veterinario vinculado

        public User() { }

        public User(string name, string username, string password, string role, Guid entityId)
        {
            Name = name;
            Username = username;
            Password = password;
            Role = role;
            EntityId = entityId;
        }

        public static void ShowInformation(User user)
        {
            Console.WriteLine($"\n👤 User ID: {user.Id}");
            Console.WriteLine($"   Name: {user.Name}");
            Console.WriteLine($"   Username: {user.Username}");
            Console.WriteLine($"   Role: {user.Role}");
            Console.WriteLine($"   Linked Entity ID: {user.EntityId}");
        }
    }
}
