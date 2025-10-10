namespace VeterinaryClinic.Models
{
    public abstract class Person
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public Person() {}
        // Constructor
        protected Person(string name, int age, string address, string phone)
        {
            Name = name;
            Age = age;
            Address = address;
            Phone = phone;
        }
    }
}