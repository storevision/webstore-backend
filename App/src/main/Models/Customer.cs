using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using BCrypt.Net;
using Webshop.Models.Cart;

namespace Webshop.Models
{
    [Table("customers")]
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        // Passwort wird als Hash gespeichert
        private string passwordHash = string.Empty;

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
                    passwordHash = value; // Bereits gehashter Wert wird übernommen
                }
                else
                {
                    // Neues Passwort wird gehasht und gespeichert
                    passwordHash = BCrypt.Net.BCrypt.HashPassword(value);
                }
            }
            get { return passwordHash; }
        }

        // Methode zum Überprüfen des Passworts
        public bool VerifyPassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new InvalidOperationException("Password hash is not set.");
            }
            
            // Passwort prüfen
            return BCrypt.Net.BCrypt.Verify(plainPassword, passwordHash);
        }

        // Parameterloser Konstruktor (wichtig für Entity Framework)
        public Customer() { }

        // Konstruktor mit Initialisierung
        public Customer(string firstName, string lastName, string email, string? phone, string? address, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
            Password = password;
        }
        
        private static bool IsBcryptHash(string input)
        {
            string bcryptPattern = @"^\$2[aby]?\$\d{2}\$[./A-Za-z0-9]{53}$";
            return System.Text.RegularExpressions.Regex.IsMatch(input, bcryptPattern);
        }
    }
}
