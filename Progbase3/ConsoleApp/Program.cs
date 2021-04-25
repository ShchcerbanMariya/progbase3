﻿using System;
using System.IO;
using Microsoft.Data.Sqlite;


namespace ConsoleApp
{
    public class User
    {
        public int id;
        public string name;
        public string password;
        public string role;
        public User()
        {
            this.id = 0;
            this.name = "";
            this.password = "";
            this.role = "user";
        }
        public User(int id, string name, string password, string role)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.role = role;
        }
        public override string ToString()
        {
            return $"{id}, {name}, {password}, {role}";
        }
    }
    public class Question
    {
        public int id;
        public string body;
        public int userAskId;
        public int mainAnswerID;
        public DateTime start;
        public DateTime end;
        public Question()
        {
            this.id = 0;
            this.body = "";
            this.userAskId = 0;
            this.mainAnswerID = 0;
        }
        public Question(int id, string body, int userAskId, int mainAnswerID, DateTime start, DateTime end)
        {
            this.id = id;
            this.body = body;
            this.userAskId = userAskId;
            this.mainAnswerID = mainAnswerID;
            this.start = start;
            this.end = end;
        }
        public override string ToString()
        {
            return $"{id},{body},{userAskId},{mainAnswerID},{start},{end}";
        }

    }
    public class Answer
    {
        public int id;
        public string body;
        public int userID;
        public bool mainAnswer;
        public int questionId;
        public DateTime time;
        public Answer()
        {
            this.id = 0;
            this.body = "";
            this.userID = 0;
            this.mainAnswer = false;
            this.questionId = 0;
        }
        public Answer(int id, string body, int userID, bool mainAnswer, int questionId, DateTime time)
        {
            this.id = id;
            this.body = body;
            this.userID = userID;
            this.mainAnswer = mainAnswer;
            this.questionId = questionId;
            this.time = time;
        }
        public override string ToString()
        {
            return $"{id}, {body}, {userID}, {questionId}, {time} ";
        }

    }


    class Program
    {
        static string GenerateString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            string finalString = new String(stringChars);
            return finalString;
        }
        static void GenerateUser(string nameFile, int data, int length, int lengthpas)
        {
            StreamWriter sw = new StreamWriter(nameFile);
            string s = "";
            for (int i = 0; i <= data; i++)
            {
                if (i == 0)
                {
                    s = "id, name, password, role";
                    sw.WriteLine(s);
                }
                else
                {
                    int num = i;
                    Random rand = new Random();
                    string role;
                    if (rand.Next(0, 5) == 0)
                    {
                        role = "moderator";
                    }
                    else
                    {
                        role = "user";
                    }
                    User newUs = new User(i, GenerateString(length), GenerateString(lengthpas), role);
                    s = newUs.ToString();
                    sw.WriteLine(s);
                }
            }
            sw.Close();
        }

        static DateTime GenerateDate(DateTime firstLim, DateTime secondLim)
        {
            Random rand = new Random();
            double limits = (secondLim - firstLim).TotalDays;
            double addDays = rand.Next(1, (int)limits);
            DateTime newDate = firstLim.AddDays(addDays);
            return newDate;
        }
        static void GenerateQuestion(string nameFile, int data, int length, DateTime firstLim, DateTime secondLim)
        {
            StreamWriter sw = new StreamWriter(nameFile);
            string s = "";
            for (int i = 0; i <= data; i++)
            {
                if (i == 0)
                {
                    s = "id, body, userAskId, mainAnswerID, start, end";
                    sw.WriteLine(s);
                }
                else
                {
                    int num = i;
                    Random rand = new Random();
                    string databaseFileName = "/home/mariya/Desktop/projects/progbase3/Progbase3/data/dataBase";
                    SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
                    connection.Open();
                    UserRepository repository = new UserRepository(connection);
                    int lastId = repository.GetLastId();
                    int generateId = rand.Next(1, lastId + 1);
                    DateTime firstDate = GenerateDate(firstLim, secondLim);
                    DateTime secondDate = GenerateDate(firstLim, secondLim);
                    while (firstDate > secondDate)
                    {
                        secondDate = GenerateDate(firstLim, secondLim);
                    }

                    Question newUs = new Question(i, GenerateString(length), generateId, 0, firstDate, secondDate);
                    s = newUs.ToString();
                    sw.WriteLine(s);
                }
            }
            sw.Close();
        }
        static void GenerateAnswer(string nameFile, int data, int length)
        {
            StreamWriter sw = new StreamWriter(nameFile);
            string s = "";
            for (int i = 0; i <= data; i++)
            {
                if (i == 0)
                {
                    s = "id, body, userID, mainAnswer, questionID, time";
                    sw.WriteLine(s);
                }
                else
                {
                    int num = i;
                    Random rand = new Random();
                    string databaseFileName = "/home/mariya/Desktop/projects/progbase3/Progbase3/data/dataBase";
                    SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
                    connection.Open();
                    UserRepository repository = new UserRepository(connection);
                    QuestionsRepository repository2 = new QuestionsRepository(connection);
                    int lastId = repository.GetLastId();
                    int generateId = rand.Next(1, lastId + 1);
                    int lastQId = repository2.GetLastId();
                    int generateQId = rand.Next(1, lastQId + 1);
                    DateTime firstDate = repository2.GetById(generateQId).start;
                    DateTime secondDate = repository2.GetById(generateQId).end;
                    DateTime time = GenerateDate(firstDate, secondDate);

                    Answer newUs = new Answer(i, GenerateString(length), generateId, false, generateQId, time);
                    s = newUs.ToString();
                    sw.WriteLine(s);
                }
            }
            sw.Close();
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Generate tables: users, questions, answers");
            string type = Console.ReadLine();
            switch (type)
            {
                case "users":
                    Console.WriteLine("Enter path to the file, number of users, length of user name, length of password: ");
                    string input = Console.ReadLine();
                    ProcessUser(input);
                    break;
                case "questions":
                    Console.WriteLine("Enter path, number of questions, length of body, first and last date");
                    string inputquest = Console.ReadLine();
                    ProcessQuestion(inputquest);
                    break;
                case "answers":
                    Console.WriteLine("Enter path, number of answers, length of body");
                    string inputanswers = Console.ReadLine();
                    ProcessAnswer(inputanswers);
                    break;
                default:
                    Console.WriteLine("incorect table");
                    break;
            }

        }
        static void ProcessUser(string input)
        {
            string[] substrings = input.Split(",");
            if (substrings.Length != 4)
            {
                Console.WriteLine("incorrect arguments");
                return;
            }
            string path = substrings[0];
            int numberOfUsers;
            bool check = int.TryParse(substrings[1], out numberOfUsers);
            if (!check || numberOfUsers <= 0)
            {
                Console.WriteLine("incorrect number of users");
                return;
            }
            int userNameLength;
            bool check2 = int.TryParse(substrings[2], out userNameLength);
            if (!check || userNameLength <= 0)
            {
                Console.WriteLine("incorrect length of name");
                return;
            }
            int passwordLength;
            bool check3 = int.TryParse(substrings[3], out passwordLength);
            if (!check || passwordLength <= 0)
            {
                Console.WriteLine("incorrect length of pass");
                return;
            }
            GenerateUser(path, numberOfUsers, userNameLength, passwordLength);
        }
        static void ProcessQuestion(string input)
        {
            //string nameFile, int data, int length, DateTime firstLim, DateTime secondLim
            string[] substrings = input.Split(",");
            if (substrings.Length != 5)
            {
                Console.WriteLine("incorrect num of arguments");
                return;
            }
            string path = substrings[0];
            int numberOfQuest;
            bool check = int.TryParse(substrings[1], out numberOfQuest);
            if (!check || numberOfQuest <= 0)
            {
                Console.WriteLine("incorrect number of users");
                return;
            }
            int qLength;
            bool check2 = int.TryParse(substrings[2], out qLength);
            if (!check || qLength <= 0)
            {
                Console.WriteLine("incorrect length of name");
                return;
            }
            DateTime first;
            try
            {
                first = DateTime.Parse(substrings[3]);
            }
            catch
            {
                Console.WriteLine("incorrect date");
                return;
            }
            DateTime second;
            try
            {
                second = DateTime.Parse(substrings[4]);
            }
            catch
            {
                Console.WriteLine("incorrect date");
                return;
            }
            if (first > second)
            {
                Console.WriteLine("incorrect date");
                return;
            }
            GenerateQuestion(path, numberOfQuest, qLength, first, second);
        }
        static void ProcessAnswer(string input)
        {
            string[] substrings = input.Split(",");
            if (substrings.Length != 3)
            {
                Console.WriteLine("incorrect num of arguments");
                return;
            }
            string path = substrings[0];
            int numberOfAnswers;
            bool check = int.TryParse(substrings[1], out numberOfAnswers);
            if (!check || numberOfAnswers <= 0)
            {
                Console.WriteLine("incorrect number of users");
                return;
            }
            int aLength;
            bool check2 = int.TryParse(substrings[2], out aLength);
            if (!check || aLength <= 0)
            {
                Console.WriteLine("incorrect length of name");
                return;
            }
            GenerateAnswer(path, numberOfAnswers, aLength);

        }
    }
}
