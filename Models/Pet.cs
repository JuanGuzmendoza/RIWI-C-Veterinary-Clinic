namespace VeterinaryClinic.Models
{
    public class Pet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }

        public Guid? OwnerId { get; set; }
        public Pet(string name, string species, string breed, string color, Guid? ownerId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Species = species;
            Breed = breed;
            Color = color;
            OwnerId = ownerId;
        }
    }
}