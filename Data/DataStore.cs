using VeterinaryClinic.Models;

namespace VeterinaryClinic.Data
{
    public static class DataStore
    {
        // Lista en memoria para guardar los pacientes
        public static List<Patient> Patients { get; set; } = new List<Patient>();

        // Aquí puedes agregar otras listas, como Pets, si luego las necesitas
        // public static List<Pet> Pets { get; set; } = new List<Pet>();
    }
}
