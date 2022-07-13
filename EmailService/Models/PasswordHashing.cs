using System.Security.Cryptography;
using System.Text;
namespace EmailService.Models
{
    public class PasswordHashing
    {
        public static string GetHash(string input) {
            if(input == null)
            {
                return null;
            }
            MD5 md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }
    }
}
