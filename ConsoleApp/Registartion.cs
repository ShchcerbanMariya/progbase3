using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using MainClassLib;
using ServiceLib;
namespace ConsoleApp
{
    public static class Authentication
    {
        static string serializedFilePath = "../ConsoleApp/serialized.xml";
        public static T Deserialize<T>(string filePath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            StreamReader reader = new StreamReader(filePath);
            T value = (T)ser.Deserialize(reader);
            reader.Close();
            return value;
        }
        public static string getHash(string str)
        {
            SHA256 sha256Hash = SHA256.Create();
            return GetHash(sha256Hash, str);
        }
        public static User AcceptRegistration(string username, string password)
        {
            RemoteService.RemoteServiceCommand("GetUserByName$" + username);
            if (/*userRepository.GetByName(username)*/  Deserialize<User>(serializedFilePath) == null && username != "" && password != "")
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
        public static User AcceptLogin(string username, string password)
        {
            SHA256 sha256Hash = SHA256.Create();
            RemoteService.RemoteServiceCommand("GetUserByName$" + username);
            User user = //userRepository.GetByName(username);
            Deserialize<User>(serializedFilePath);
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