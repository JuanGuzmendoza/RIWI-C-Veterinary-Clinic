using Helpers;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Services
{
    public static class PetService
    {
        public static List<Pet> RegisterPet(Patient patient)
        {
            List<Pet> pets = new List<Pet>();
            bool addMore = true;
            while (addMore)
            {
                Console.WriteLine("--- Register New Pet ---");
                string name = Validations.ValidateContent("Enter pet's name: ");
                string species = Validations.ValidateContent("Enter pet's species: ");
                string breed = Validations.ValidateContent("Enter pet's breed: ");
                string color = Validations.ValidateContent("Enter pet's color: ");

                Pet newPet = new Pet(name, species, breed, color, patient);
                pets.Add(newPet);
                Console.WriteLine("Pet registered successfully!\n");

                string more = Validations.ValidateContent("Do you want to add another pet? (y/n): ");
                if (!more.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    addMore = false;
                }
            }
            return pets;
        }

        public static void ListPets(List<Pet> pets)
        {
            Console.WriteLine("--- Pet List ---");
            foreach (var pet in pets)
            {
                Console.WriteLine($"Name: {pet.Name}, Species: {pet.Species}, Breed: {pet.Breed}, Color: {pet.Color}");
            }
            Console.WriteLine();
        }

        public static void FindPetByName(List<Pet> pets, string name)
        {
            var pet = pets.Find(p => p.Name != null && p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (pet != null)
            {
                Console.WriteLine($"Found: Name: {pet.Name}, Species: {pet.Species}, Breed: {pet.Breed}, Color: {pet.Color}\n");
            }
            else
            {
                Console.WriteLine("Pet not found.\n");
            }
        }
    }
}
