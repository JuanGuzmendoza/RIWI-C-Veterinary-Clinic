namespace VeterinaryClinic.Models
{
    public class Patient
    {
        public Guid Id { get; private set; } 
        public string Name { get; set; }
        public int Age { get; set; }
        public string? Symptom { get; set; }

        public Patient(string name, int age, string? symptom)
        {
            Id = Guid.NewGuid();
            Name = name;
            Age = age;
            Symptom = symptom;
        }
    }
}
