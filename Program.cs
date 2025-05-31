// See https://aka.ms/new-console-template for more information
using tpmodul15_103022300082;

public class Program
{
    public static void Main(string[] args)
    {
        UjiInputJSON ujiInput = new UjiInputJSON();
        ujiInput.GetUserInput();
        ujiInput.SaveConfig();
        ujiInput.LoadConfig();
        Console.WriteLine("Data has been saved to TesInput.json");
    }
}