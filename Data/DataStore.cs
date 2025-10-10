using System.Collections.Generic;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Data
{
    public static class DataStore
    {
        // 🔹 Diccionarios cargados desde Firebase
        public static Dictionary<string, Customer> Customers { get; set; } = new();
        public static Dictionary<string, Pet> Pets { get; set; } = new();
        public static Dictionary<string, User> Users { get; set; } = new();
        public static Dictionary<string, Veterinarian> Veterinarians { get; set; } = new();

        // 🔹 Método para limpiar la memoria (por ejemplo, al cerrar sesión)
        public static void Clear()
        {
            Customers.Clear();
            Pets.Clear();
            Users.Clear();
        }
    }
}
