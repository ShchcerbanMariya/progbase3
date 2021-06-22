using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp
{
    public static class Authentication
    {
        public static string getHash(string str)
        {
            SHA256 sha256Hash = SHA256.Create();
            return GetHash(sha256Hash, str);
        }
        public static User AcceptRegistration(UserRepository userRepository, string username, string password)
        {
            if (userRepository.GetByName(username) == null && username != "" && password != "")
            {
                SHA256 sha256Hash = SHA256.Create();
                string passHash = GetHash(sha256Hash, password);
                User user = new User();
                user.name = username;
                user.password = passHash;
                return user;
            }
            else
            {
                return null;
            }
        }
        public static User AcceptLogin(UserRepository userRepository, string username, string password)
        {
            SHA256 sha256Hash = SHA256.Create();
            User user = userRepository.GetByName(username);
            if(user != null)
            {
                if (VerifyHash(sha256Hash, password, user.password))
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            var hashOfInput = GetHash(hashAlgorithm, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }

}
