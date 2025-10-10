namespace VeterinaryClinic.Models
{
    using VeterinaryClinic.Interfaces;
    using System;
    using System.Collections.Generic;

    public class Veterinarian : Person, IEntity
    {
        public string Specialization { get; set; }
        public List<Guid> ConsultationIds { get; set; }

        // Constructor
        public Veterinarian(string name, int age, string address , string phone , string specialization)
            : base(name, age, address, phone)
        {
            Specialization = specialization;
            ConsultationIds = new List<Guid>();
        }

        // Optional method to show information (like you did in Customer)
        public static void ShowInformation(Veterinarian vet)
        {
            Console.WriteLine($"\n🩺 Veterinarian ID: {vet.Id}");
            Console.WriteLine($"   Name: {vet.Name}");
            Console.WriteLine($"   Age: {vet.Age}");
            Console.WriteLine($"   Address: {vet.Address}");
            Console.WriteLine($"   Phone: {vet.Phone}");
            Console.WriteLine($"   Specialization: {vet.Specialization}");

            if (vet.ConsultationIds != null && vet.ConsultationIds.Count > 0)
            {
                Console.WriteLine("Consultations:");
                foreach (var consultationId in vet.ConsultationIds)
                {
                    Console.WriteLine($"  - Consultation ID: {consultationId}");
                }
            }
            else
            {
                Console.WriteLine("Consultations: None");
            }
        }
    }
}
