using System.Security.Cryptography;
using System.Text;

namespace RentalVehicle.PasswordHash
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            if(string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = Encoding.UTF8.GetBytes(password);
                var hashPassword = sha256.ComputeHash(hashedBytes);
                var hash = BitConverter.ToString(hashPassword).Replace("-", "").ToLower();
                return hash;
            }
        }
        
    }
}
