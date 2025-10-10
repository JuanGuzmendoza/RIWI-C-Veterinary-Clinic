using VeterinaryClinic.Data;
using VeterinaryClinic.Models;

namespace Helpers
{
    public static class FirebaseFinder
    {

        public static string? GetFirebaseIdByName(
       string name,
       Dictionary<string, dynamic> dictionary)
        {
            return dictionary
                .FirstOrDefault(x =>
                    x.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Key;
        }

    }
}
