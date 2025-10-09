using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;
using Helpers;

namespace VeterinaryClinic.Services
{
    public static class PatientService
    {
        private static readonly PatientRepository _repository = new PatientRepository();

        public static async Task RegisterPatientAsync()
        {
            Console.WriteLine("--- Register New Patient ---");

            string name = Validations.ValidateContent("Enter patient's name: ");
            int age = Validations.ValidateNumber("Enter patient's age: ");

            Patient newPatient = new Patient(name, age);

            List<Pet> newPets = PetService.RegisterPet(newPatient);
            newPatient.Pets = newPets;

            string generatedId = await _repository.CrearAsync(newPatient);

            Console.WriteLine($"✅ Patient registered successfully with ID: {generatedId}\n");
        }

        public static async Task ListPatientsAsync()
        {
            Console.WriteLine("--- Patient List ---");

            var patientsDict = await _repository.ObtenerTodosAsync();

            if (patientsDict == null || patientsDict.Count == 0)
            {
                Console.WriteLine("No patients found.\n");
                return;
            }

            foreach (var entry in patientsDict)
            {
                var patient = entry.Value;
                Console.WriteLine($"Id: {entry.Key}, Name: {patient.Name}, Age: {patient.Age}");

                if (patient.Pets != null)
                {
                    foreach (var pet in patient.Pets)
                    {
                        Console.WriteLine($"Pet -> Name: {pet.Name}, Species: {pet.Species}, Breed: {pet.Breed}, Color: {pet.Color}");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }



    }
}
