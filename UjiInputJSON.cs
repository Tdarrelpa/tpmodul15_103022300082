using System.Text.Json;
using System.Text.Json.Serialization;

namespace tpmodul15_103022300082
{
    public class Rootobject
    {
        [JsonPropertyName("Nama")]
        public string? Nama { get; set; }
        
        [JsonPropertyName("Umur")]
        public int Umur { get; set; }
    }

    public class UjiInputJSON
    {
        private readonly Rootobject objekJSON;
        private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "TesInput.json");

        public UjiInputJSON()
        {
            objekJSON = new Rootobject();
        }

        public void GetUserInput()
        {
            do
            {
                try
                {
                    Console.Write("Enter your name (A-Z, a-z): ");
                    string? nameInput = Console.ReadLine();
                    ValidateName(nameInput);
                    if (nameInput == null) { throw new ArgumentNullException(nameof(nameInput), "Name input cannot be null."); }
                    objekJSON.Nama = nameInput;

                    Console.Write("Enter your age (5-120): ");
                    string? ageInput = Console.ReadLine();
                    ValidateAge(ageInput);
                    if (ageInput == null) { throw new ArgumentNullException(nameof(ageInput), "Age input cannot be null."); }
                    objekJSON.Umur = int.Parse(ageInput);
                    break;
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine(ex.Message);
                    return; // Exit the method if input is null
                }
            } while (true);
        }

        private static void ValidateName(string? name)
        {
            try
            {
                // Mengecek input kosong
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Name cannot be empty. Please enter a valid name.");
                }
                // Mengecek jika input hanya huruf (A-Z, a-z)
                if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[A-Za-z]+$"))
                {
                    throw new ArgumentException("Invalid name. Please enter a valid name using only letters.");
                }
                // Jika semua validasi lolos
                Console.WriteLine("Input valid, lanjutkan proses.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                throw; // Mengembalikan kontrol ke pemanggil jika terjadi kesalahan
            }
        }

        private static void ValidateAge(string? age)
        {
            try
            {
                // Mengecek input kosong
                if (string.IsNullOrWhiteSpace(age))
                {
                    throw new ArgumentException("Age cannot be empty. Please enter a valid age.");
                }
                // Mengecek input hanya angka
                if (!System.Text.RegularExpressions.Regex.IsMatch(age, @"^\d+$"))
                {
                    throw new ArgumentException("Invalid age format. Please enter a numeric value.");
                }
                // Mengecek rentang umur valid (5-120 tahun)
                if (!int.TryParse(age, out int ageValue) || ageValue < 5 || ageValue > 120)
                {
                    throw new ArgumentException("Invalid age. Please enter an age between 5 and 120.");
                }
                Console.WriteLine("Input valid, lanjutkan proses.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return; // Mengembalikan kontrol ke pemanggil jika terjadi kesalahan
            }
        }

        // Serialize options for JSON output
        private static readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        public void LoadConfig()
        {
            if (!File.Exists(FilePath))
            {
                SaveConfig();
                Console.WriteLine("File tidak ditemukan, membuat file baru");
            }

            try
            {
                var configJson = File.ReadAllText(FilePath);
                var configFromFile = JsonSerializer.Deserialize<UjiInputJSON>(configJson);
                objekJSON.Nama = configFromFile?.Dapat_Nama() ?? string.Empty;
                objekJSON.Umur = configFromFile?.Dapat_Umur() ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan: {ex.Message}");
            }
        }

        public void SaveConfig()
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(this, options);
                File.WriteAllText(FilePath, jsonString);
                Console.WriteLine("Berhasil menyimpan konfigurasi baru");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gagal menyimpan konfigurasi baru: {ex.Message}");
            }
        }

        private string? Dapat_Nama() { return objekJSON.Nama; }
        private int Dapat_Umur() { return objekJSON.Umur; }
    }
}