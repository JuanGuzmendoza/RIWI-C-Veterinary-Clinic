namespace VeterinaryClinic.Models
{
    using VeterinaryClinic.Interfaces;
    using System;
    using System.Collections.Generic;

    public class Customer : Person, IEntity
    {
        public List<Guid> PetIds { get; set; } = new();

        // Empty constructor for deserialization
        public Customer() {}

        // Constructor for creating a new Customer
        public Customer(string name, int age, string address = "", string phone = "")
            : base(name, age, address, phone)
        {
            PetIds = new List<Guid>();
        }

        public static void ShowInformation(Customer customer)
        {
            Console.WriteLine($"\n--- CUSTOMER INFO ---");
            Console.WriteLine($"ID      : {customer.Id}");
            Console.WriteLine($"Name    : {customer.Name}");
            Console.WriteLine($"Age     : {customer.Age}");
            Console.WriteLine($"Address : {customer.Address}");
            Console.WriteLine($"Phone   : {customer.Phone}");

            if (customer.PetIds?.Count > 0)
                Console.WriteLine("Pets IDs: " + string.Join(", ", customer.PetIds));
            else
                Console.WriteLine("Pets    : None");
        }
    }
}
