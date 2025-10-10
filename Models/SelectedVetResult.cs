namespace VeterinaryClinic.Models
{
    public class SelectedVetResult
    {
        public Guid SelectedVeterinarianId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
