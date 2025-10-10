using System;
using VeterinaryClinic.Interfaces;

namespace VeterinaryClinic.Models
{
    public class Consultation : IEntity
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid VeterinarianId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        // Optional: useful if you want to store diagnosis or notes later
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }

        // ✅ Constructor
        public Consultation(Guid customerId, Guid veterinarianId, string description)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            VeterinarianId = veterinarianId;
            Description = description;
            Date = DateTime.Now;
        }

        // Empty constructor (required for serialization or Firebase)
        public Consultation() { }

        // 🧾 Optional helper to display info in console
        public static void ShowInformation(Consultation consultation)
        {
            Console.WriteLine($"\n🧾 Consultation ID: {consultation.Id}");
            Console.WriteLine($"   Customer ID: {consultation.CustomerId}");
            Console.WriteLine($"   Veterinarian ID: {consultation.VeterinarianId}");
            Console.WriteLine($"   Description: {consultation.Description}");
            Console.WriteLine($"   Date: {consultation.Date}");
            
            if (!string.IsNullOrEmpty(consultation.Diagnosis))
                Console.WriteLine($"   Diagnosis: {consultation.Diagnosis}");
            
            if (!string.IsNullOrEmpty(consultation.Notes))
                Console.WriteLine($"   Notes: {consultation.Notes}");
        }
    }
}
