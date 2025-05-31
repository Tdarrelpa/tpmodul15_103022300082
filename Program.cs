// See https://aka.ms/new-console-template for more information
using tpmodul15_103022300082;

public class Program
{
    public static void Main(string[] args)
    {
        UjiInputJSON ujiInput = new();
        ujiInput.LoadConfig();

        Console.Write("Enter your name (A-Z, a-z): ");
        string? nameInput = Console.ReadLine();
        ujiInput.GetUserNameInput(nameInput);


        Console.Write("Enter your age (5-120): ");
        string? ageInput = Console.ReadLine();
        ujiInput.GetUserAgeInput(ageInput);

        Console.Write($"Nama: {ujiInput.Dapat_Nama()} \nUmur: {ujiInput.Dapat_Umur()} \n");
        Console.WriteLine("Data has been saved to TesInput.json");
    }
}