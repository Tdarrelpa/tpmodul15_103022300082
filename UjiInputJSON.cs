using System.Text.Json;
using System.Text.Json.Serialization;

namespace tpmodul15_103022300082
{
    public class Rootobject
    {
        [JsonPropertyName("nama")]
        public string? Nama { get; set; }
        
        [JsonPropertyName("umur")]
        public int Umur { get; set; }
    }

    public class UjiInputJSON
    {
        private readonly Rootobject objekJSON;
        private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "TesInput.json");
        private static bool isNameValid = false;
        private static bool isAgeValid = false;

        public UjiInputJSON()
        {
            objekJSON = new Rootobject();
        }

        // Dipisah menjadi 2 method untuk memisahkan input nama dan umur
        public void GetUserNameInput(string? nameInput)
        {
            try
            {
                ValidateName(nameInput);
                if (nameInput == null) { throw new ArgumentNullException(nameof(nameInput), "Name input cannot be null."); }
                objekJSON.Nama = nameInput;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                return; // Exit the method if input is null
            }
        }

        public void GetUserAgeInput(string? ageInput)
        {
            try
            {
                ValidateAge(ageInput);
                // input null sudah ditangani di StoIParse(), tidak perlu ada pengecekan null lagi
                objekJSON.Umur = StoIParse(ageInput); //Pengganti 'int.Parse()' dan Convert.ToInt32(), karena harus menggunakan catch yang dicomment biar jalan
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                return; // Exit the method if input is null
            }
            /*
            catch (FormatException ex) // menangani kesalahan format
            {
                Console.WriteLine($"Invalid format for age: {ex.Message}");
                return; // Exit the method if input is not a valid integer
            }
            catch (OverflowException ex) // menangani kesalahan overflow jika angka terlalu besar atau kecil
            {
                Console.WriteLine($"Buffer overflow for age: {ex.Message}");
                return; // Exit the method if input is out of range
            }
            */
        }

        /* 
         * Dibuat private untuk menghindari akses langsung dari luar kelas, 
         * dan static untuk menghindari pembuatan instance setiap kali validasi diperlukan
         */
        private static void ValidateName(string? name)
        {
            try
            {
                // Mengecek input kosong
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Name cannot be empty, please enter a valid name!");
                }
                // Mengecek jika input hanya huruf (A-Z, a-z)
                if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[A-Za-z]+$"))
                {
                    throw new ArgumentException("Invalid name, please enter a valid name using only letters!");
                }
                // Jika semua validasi lolos
                Console.WriteLine("Input valid, lanjutkan proses!");
                isNameValid = true; // Set validasi nama berhasil
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return; // Mengembalikan kontrol ke pemanggil jika terjadi kesalahan
            }
        }

        private static void ValidateAge(string? age)
        {
            try
            {
                // Mengecek input kosong
                if (string.IsNullOrWhiteSpace(age))
                {
                    throw new ArgumentException("Age cannot be empty, please enter a valid age!");
                }
                // Mengecek input hanya angka
                if (!System.Text.RegularExpressions.Regex.IsMatch(age, @"^\d+$"))
                {
                    throw new ArgumentException("Invalid age format, please enter a numeric value!");
                }
                // Mengecek rentang umur valid (5-120 tahun)
                if (!int.TryParse(age, out int ageValue) || ageValue < 5 || ageValue > 120)
                {
                    throw new ArgumentException("Invalid age, please enter an age between 5 and 120!");
                }
                Console.WriteLine("Input valid, lanjutkan proses.");
                isAgeValid = true; // Set validasi nama berhasil
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return; // Mengembalikan kontrol ke pemanggil jika terjadi kesalahan
            }
        }

        // Membuat fungsi untuk mengkonversi string ke integer dengan penanganan kesalahan
        private static int StoIParse(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input), "Input cannot be null or empty.");
            }
            
            if (!int.TryParse(input, out int result))
            {
                // throw new FormatException("Input is not a valid integer.");
                return 0; // Cukup kembalikan 0, implementasi sebenarnya ada di ValidateAge()
            }
            
            return result;
        }

        /* 
         * Menurut standar C#, ini wajib dideklarasikan secara terpisah untuk menghindari pembuatan instance per eksekusi kode (static),
         * diakses dari luar kelas UjiInputJSON (private),
         * dan modifikasi hanya dilakukan sekali saat kelas diinisialisasi (readonly).
         * karena JsonSerializerOptions tidak perlu diubah selama runtime.
         */
        private static readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        // Memuat konfigurasi file JSON
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
                Console.WriteLine($"Terjadi kesalahan saat memuat data: {ex.Message}");
            }
        }

        // Menyimpan konfigurasi ke file JSON
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

        // Keempat getter ini dipanggil dalam program main, jadi harus dibuat public
        public string? Dapat_Nama() { return objekJSON.Nama; }
        public int Dapat_Umur() { return objekJSON.Umur; }

        public bool IsNameValid() { return isNameValid; }
        public bool IsAgeValid(){return isAgeValid;}
    }
}