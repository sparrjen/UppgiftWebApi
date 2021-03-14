using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

#nullable disable

namespace UppgiftWebApi.Entities
{
    public partial class Administrator
    {
        public Administrator()
        {
            Cases = new HashSet<Case>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] AdminHash { get; set; }
        public byte[] AdminSalt { get; set; }

        public virtual ICollection<Case> Cases { get; set; }

        public void CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                AdminSalt = hmac.Key;
                AdminHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool ValidatePasswordHash(string password)
        {

            using (var hmac = new HMACSHA512(AdminSalt))
            {

                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != AdminHash[i])
                        return false;
                }
            }

            return true;
        }
    }
}
