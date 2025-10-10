        namespace VeterinaryClinic.Models
        {
            public class Patient
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public int Age { get; set; }
                public List<Guid> PetIds { get; set; }

                public Patient(string name, int age)
                {
                    Id = Guid.NewGuid();
                    Name = name;
                    Age = age;
                    PetIds = new List<Guid>();
                }

            }
        }