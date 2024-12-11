using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webshop.Models.Cart;

namespace Webshop.App.src.main.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column ("id")]
        public int CustomerID { get; set; }
        
        [Required]
        [Column ("email")] 
        // UNIQUE!!!
        public string? Email { get; set; }
        
        [Required]
        [Column ("display_name")]
        public string  DisplayName { get; set; }

        [Required]
        [Column("password_hash")]
        // Passwort wird als Hash gespeichert
        private string PasswordHash = string.Empty;

        [Required]
        [Column("password_changed_at")]
        public DateTime  PasswordChangedAt { get; set; } = DateTime.Now;
        
        public ICollection<Order>? Orders { get; set; } = null!;

        // Passwort-Property: nur zum Setzen
        public string Password
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Password cannot be empty or null.");
                }

                // Prüfen, ob das Passwort bereits ein gültiger bcrypt-Hash ist
                if (IsBcryptHash(value))
                {
                    PasswordHash = value; // Bereits gehashter Wert wird übernommen
                }
                else
                {
                    // Neues Passwort wird gehasht und gespeichert
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(value);
                }
            }
            get => PasswordHash;
        }

        // Methode zum Überprüfen des Passworts
        public bool VerifyPassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(PasswordHash))
            {
                throw new InvalidOperationException("Password hash is not set.");
            }
            // Passwort prüfen
            return BCrypt.Net.BCrypt.Verify(plainPassword, PasswordHash);
        }

        // Parameterloser Konstruktor (wichtig für Entity Framework)
        public User(string displayName)
        {
            DisplayName = displayName;
        }

        // Konstruktor mit Initialisierung
        public User(string email, string password, string displayName)
        {
            Email = email;
            Password = password;
            DisplayName = displayName;
        }
        
        private static bool IsBcryptHash(string input)
        {
            string bcryptPattern = @"^\$2[aby]?\$\d{2}\$[./A-Za-z0-9]{53}$";
            return System.Text.RegularExpressions.Regex.IsMatch(input, bcryptPattern);
        }
    }
}
