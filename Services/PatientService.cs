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
            string symptom = Validations.ValidateContent("Enter patient's symptom: ");

            Patient newPatient = new Patient(name, age, symptom);
            list.Add(newPatient);
            Console.WriteLine("Patient registered successfully!\n");
        }

        public static void ListPatients(List<Patient> list)
        {
            Console.WriteLine("--- Patient List ---");
            foreach (var patient in list)
            {
                Console.WriteLine($"Id: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Symptom: {patient.Symptom}");
            }
            Console.WriteLine();
        }

        public static void FindPatientByName(List<Patient> list, string name)
        {
            var patient = list.Find(p => p.Name != null && p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (patient != null)
            {
                Console.WriteLine($"Found: Id: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Symptom: {patient.Symptom}\n");
            }
            else
            {
                Console.WriteLine("Patient not found.\n");
            }
        }
    }
}
