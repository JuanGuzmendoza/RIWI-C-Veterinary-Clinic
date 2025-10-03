    using VeterinaryClinic.Models;
    using VeterinaryClinic.Services;
    using Helpers;

    List<Patient> patients = new List<Patient>();
    List<Pet> pets = new List<Pet>();

    Dictionary<Guid, Pet> patientPets = new Dictionary<Guid, Pet>();

    bool exit = false;

    while (!exit)
    {
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
                PatientService.RegisterPatient(patients);
                break;
            case "2":
                PatientService.ListPatients(patients);
                break;
            case "3":
                string name = Validations.ValidateContent("Enter patient's name to search: ");
                PatientService.FindPatientByName(patients, name);
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