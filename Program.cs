using VeterinaryClinic.Services;

class Program
{
    static async Task Main(string[] args)
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("---- Veterinary Clinic Menu ----");
            Console.WriteLine("1. Register Patient");
            Console.WriteLine("2. List Patients");
            Console.WriteLine("3. Find Patient by Name");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option: ");
            string? option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.Clear();
                    await PatientService.RegisterPatientAsync();
                    break;
                case "2":
                    Console.Clear();
                    await PatientService.ListPatientsAsync();
                    break;
                case "3":
                    Console.Clear();
                    // await PatientService.FindPatientByNameAsync();
                    break;
                case "4":
                    exit = true;
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Try again.\n");
                    break;
            }
        }
    }
}
