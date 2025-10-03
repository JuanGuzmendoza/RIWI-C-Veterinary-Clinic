using VeterinaryClinic.Models;
using Helpers;
namespace VeterinaryClinic.Services
{
    public static class PatientService
    {
        public static void RegisterPatient(List<Patient> list)
        {

            Console.WriteLine("--- Register New Patient ---");
            string name = Validations.ValidateContent("Enter patient's name: ");
            int age = Validations.ValidateNumber("Enter patient's age: ");

            Patient newPatient = new Patient(name, age);

            List<Pet> newPets=PetService.RegisterPet(newPatient);

            newPatient.Pets = newPets;

            list.Add(newPatient);
           
        }

        public static void ListPatients(List<Patient> list)
        {
            Console.WriteLine("--- Patient List ---");
            foreach (var patient in list)
            {
                Console.WriteLine($"Id: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}");
                if (patient.Pets != null)
                {
                    foreach (var pet in patient.Pets)
                    {
                        Console.WriteLine($"Pet -> Name: {pet.Name}, Species: {pet.Species}, Breed: {pet.Breed}, Color: {pet.Color}");
                    }
                }
            }
            Console.WriteLine();
        }

        public static void FindPatientByName(List<Patient> list, string name)
        {
            var patient = list.Find(p => p.Name != null && p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (patient != null)
            {
                Console.WriteLine($"Found: Id: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}");

                foreach (var pet in patient.Pets)
                {
                    Console.WriteLine($"Pet -> Name: {pet.Name}, Species: {pet.Species}, Breed: {pet.Breed}, Color: {pet.Color}");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Patient not found.\n");
            }
        }
    }
}
