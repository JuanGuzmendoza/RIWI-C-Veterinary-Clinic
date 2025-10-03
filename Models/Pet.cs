namespace VeterinaryClinic.Models
{
    public class Pet
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }

        public Patient? Owner { get; set; }
        public Pet(string name, string species, string breed, string color, Patient? owner)
        {
            Name = name;
            Species = species;
            Breed = breed;
            Color = color;
            Owner = owner;
        }
    }
}