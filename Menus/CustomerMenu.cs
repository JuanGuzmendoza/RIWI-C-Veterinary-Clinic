using VeterinaryClinic.Models;
namespace VeterinaryClinic.Menus
{

    public static class CustomerMenu
    {
        public static async Task ShowAsync(User user)
        {
            Console.WriteLine($"🐶 Welcome {user.Name}, Customer Menu");
            await Task.Delay(1000);
        }
    }


}
