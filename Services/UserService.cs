using System;
using System.Linq;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;
using Helpers;
using VeterinaryClinic.Data;

namespace VeterinaryClinic.Services
{
    public static class UserService
    {
        private static readonly UserRepository _userRepository = new();

        // ✅ CREATE USER
        public static async Task RegisterAsync()
        {
            Console.WriteLine("--- 👤 Register New User ---");

            string name = Validations.ValidateContent("Enter user's full name: ");
            string username = Validations.ValidateContent("Enter username: ");
            string password = Validations.ValidateContent("Enter password: ");

            // cargar usuarios en memoria si está vacío
            if (DataStore.Users == null || DataStore.Users.Count == 0)
                DataStore.Users = await _userRepository.ObtenerTodosAsync();

            // verificar username duplicado
            bool usernameExists = DataStore.Users.Values
                .Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (usernameExists)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Username already exists. Choose a different username.");
                Console.ResetColor();
                return;
            }

            // ✅ Choose role
            string role = "";
            while (true)
            {
                Console.Write("Enter role (Customer / Veterinarian): ");
                role = Console.ReadLine()?.Trim() ?? "";

                if (role.Equals("Customer", StringComparison.OrdinalIgnoreCase) ||
                    role.Equals("Veterinarian", StringComparison.OrdinalIgnoreCase))
                    break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Invalid role. Please enter 'Customer' or 'Veterinarian'.");
                Console.ResetColor();
            }

            // ✅ Create the linked entity by calling the service (returns Guid)
            Guid entityId;
            if (role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
            {
                entityId = await CustomerService.RegisterAsync();
            }
            else
            {
                entityId = await VeterinarianService.RegisterAsync();
            }

            // ✅ Create the User and link the entity
            var newUser = new User(name, username, password, role, entityId);
            string userFirebaseId = await _userRepository.CrearAsync(newUser);

            // guardar en memoria local
            DataStore.Users[userFirebaseId] = newUser;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✅ User registered successfully!");
            Console.WriteLine($"🆔 User Firebase ID: {userFirebaseId}");
            Console.WriteLine($"👤 Name: {newUser.Name}");
            Console.WriteLine($"📛 Username: {newUser.Username}");
            Console.WriteLine($"🔑 Role: {newUser.Role}");
            Console.WriteLine($"🔗 Linked Entity ID: {newUser.EntityId}");
            Console.ResetColor();
        }

        // ✅ READ ALL USERS
        public static async Task ListAsync()
        {
            Console.WriteLine("--- 📋 User List ---\n");

            if (DataStore.Users == null || DataStore.Users.Count == 0)
                DataStore.Users = await _userRepository.ObtenerTodosAsync();

            if (DataStore.Users == null || DataStore.Users.Count == 0)
            {
                Console.WriteLine("⚠️ No users found.\n");
                return;
            }

            foreach (var entry in DataStore.Users)
            {
                var id = entry.Key;
                var user = entry.Value;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"🆔 Firebase ID : {id}");
                Console.ResetColor();

                Console.WriteLine($"👤 Name         : {user.Name}");
                Console.WriteLine($"📛 Username     : {user.Username}");
                Console.WriteLine($"🔑 Role         : {user.Role}");
                Console.WriteLine($"🔗 Linked Entity: {user.EntityId}");
                Console.WriteLine(new string('-', 40));
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        // ✅ SHOW ONE USER (buscar por username o name)
        public static async Task ShowAsync()
        {
            string input = Validations.ValidateContent("Enter user's username or name: ");

            if (DataStore.Users == null || DataStore.Users.Count == 0)
                DataStore.Users = await _userRepository.ObtenerTodosAsync();

            var match = DataStore.Users
                .FirstOrDefault(u =>
                    u.Value.Username.Equals(input, StringComparison.OrdinalIgnoreCase) ||
                    u.Value.Name.Equals(input, StringComparison.OrdinalIgnoreCase));

            if (match.Value == null)
            {
                Console.WriteLine("❌ User not found.");
                return;
            }

            User.ShowInformation(match.Value);
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        // ✅ UPDATE USER (buscar por username o name)
        public static async Task UpdateAsync()
        {
            string input = Validations.ValidateContent("Enter user's username or name to update: ");

            if (DataStore.Users == null || DataStore.Users.Count == 0)
                DataStore.Users = await _userRepository.ObtenerTodosAsync();

            var match = DataStore.Users
                .FirstOrDefault(u =>
                    u.Value.Username.Equals(input, StringComparison.OrdinalIgnoreCase) ||
                    u.Value.Name.Equals(input, StringComparison.OrdinalIgnoreCase));

            if (match.Value == null)
            {
                Console.WriteLine("❌ User not found.");
                return;
            }

            string firebaseId = match.Key;
            var existing = match.Value;

            // pedir nuevos datos (si quieres mantener el viejo, simplemente presiona Enter)
            string newName = Validations.ValidateContent($"Enter new name (current: {existing.Name}): ");
            string newUsername = Validations.ValidateContent($"Enter new username (current: {existing.Username}): ");
            string newPassword = Validations.ValidateContent("Enter new password: ");
            string newRole = Validations.ValidateContent($"Enter new role (Customer / Veterinarian) (current: {existing.Role}): ");

            // verificar duplicado de username (si cambió)
            if (!newUsername.Equals(existing.Username, StringComparison.OrdinalIgnoreCase))
            {
                bool usernameExists = DataStore.Users.Values
                    .Any(u => u.Username.Equals(newUsername, StringComparison.OrdinalIgnoreCase));
                if (usernameExists)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ That username is already taken by another user.");
                    Console.ResetColor();
                    return;
                }
            }

            existing.Name = newName;
            existing.Username = newUsername;
            existing.Password = newPassword;
            existing.Role = newRole;

            await _userRepository.ActualizarAsync(firebaseId, existing);

            // actualizar en memoria
            DataStore.Users[firebaseId] = existing;

            Console.WriteLine("✅ User updated successfully!");
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        // ✅ DELETE USER (buscar por username o name)
        public static async Task DeleteAsync()
        {
            string input = Validations.ValidateContent("Enter user's username or name to delete: ");

            if (DataStore.Users == null || DataStore.Users.Count == 0)
                DataStore.Users = await _userRepository.ObtenerTodosAsync();

            var match = DataStore.Users
                .FirstOrDefault(u =>
                    u.Value.Username.Equals(input, StringComparison.OrdinalIgnoreCase) ||
                    u.Value.Name.Equals(input, StringComparison.OrdinalIgnoreCase));

            if (match.Value == null)
            {
                Console.WriteLine("❌ User not found.");
                return;
            }

            string firebaseId = match.Key;

            await _userRepository.EliminarAsync(firebaseId);
            DataStore.Users.Remove(firebaseId);

            Console.WriteLine($"🗑️ User '{match.Value.Username}' deleted successfully!");
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }
    }
}
