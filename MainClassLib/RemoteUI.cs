using Microsoft.Data.Sqlite;
using static System.Console;
using System.Net;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ServiceLib;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
namespace MainClassLib
{
    public class RemoteUI
    {
        static SqliteConnection connection;
        static string serializedFilePath = "../ConsoleApp/serialized.xml";
        static string databaseFile;
        static void StartNewThread(object obj)
        {
            Socket socket = (Socket)obj;
            ProcessClient(socket);
            socket.Close();
            WriteLine("Connection closed");
        }
        static void ProcessClient(Socket newClient)
        {
            byte[] buffer = new byte[1024];

            IService service = new Service(connection, databaseFile);
            while (true)
            {
                try
                {
                    WriteLine("Server is waiting for a message....");

                    int nBytes = newClient.Receive(buffer);
                    string inputText = Encoding.ASCII.GetString(buffer, 0, nBytes);

                    string[] command = inputText.Split('$');
                    if (command[0].Equals("GetAnswerById"))
                    {
                        Answer answer = service.GetAnswerById(int.Parse(command[1]));
                        buffer = Serialize<Answer>(answer, serializedFilePath);
                    }
                    else if (command[0].Equals("DeleteAnswerById"))
                    {
                        int answer = service.DeleteAnswerById(int.Parse(command[1]));
                        buffer = Serialize<int>(answer, serializedFilePath);
                    }
                    else if (command[0].Equals("InsertANswer"))
                    {
                        Answer answer = Deserialize<Answer>(serializedFilePath);
                        int result = service.InsertANswer(answer);
                        buffer = Serialize<int>(result, serializedFilePath);
                    }
                    else if (command[0].Equals("DeleteAllAnswersOfQuestion"))
                    {
                        int q = service.DeleteAllAnswersOfQuestion(int.Parse(command[1]));
                        buffer = Serialize<int>(q, serializedFilePath);
                    }
                    else if (command[0].Equals("GetTotalAnswerPages"))
                    {
                        int page = service.GetTotalAnswerPages();
                        buffer = Serialize<int>(page, serializedFilePath);
                    }
                    else if (command[0].Equals("GetAllUserAns"))
                    {
                        List<Answer> answers = service.GetAllUserAns(int.Parse(command[1]));
                        buffer = Serialize<List<Answer>>(answers, serializedFilePath);
                    }
                    else if (command[0].Equals("GetByQuestionId"))
                    {
                        List<Answer> answers = service.GetByQuestionId(int.Parse(command[1]));
                        buffer = Serialize<List<Answer>>(answers, serializedFilePath);
                    }
                    else if (command[0].Equals("GetMainAnswer"))
                    {
                        Answer answer = service.GetMainAnswer(int.Parse(command[1]));
                        buffer = Serialize<Answer>(answer, serializedFilePath);
                    }
                    else if (command[0].Equals("UpdateAnswer"))
                    {
                        Answer answer = Deserialize<Answer>(serializedFilePath);
                        int res = service.UpdateAnswer(answer);
                        buffer = Serialize<int>(res, serializedFilePath);
                    }
                    else if (command[0].Equals("GetQuestionById"))
                    {
                        Question res = service.GetQuestionById(int.Parse(command[1]));
                        buffer = Serialize<Question>(res, serializedFilePath);
                    }
                    else if (command[0].Equals("GetAllQuestions"))
                    {
                        List<Question> res = service.GetAllQuestions();
                        buffer = Serialize<List<Question>>(res, serializedFilePath);
                    }
                    else if (command[0].Equals("GetAllContains"))
                    {
                        List<Question> res = service.GetAllContains(command[1]);
                        buffer = Serialize<List<Question>>(res, serializedFilePath);
                    }
                    else if (command[0].Equals("DeleteQuestionById"))
                    {
                        int res = service.DeleteQuestionById(int.Parse(command[1]));
                        buffer = Serialize<int>(res, serializedFilePath);
                    }
                     else if (command[0].Equals("InsertQuestion"))
                    {
                        Question question = Deserialize<Question>(serializedFilePath);
                        int res = service.InsertQuestion(question);
                        buffer = Serialize<int>(res, serializedFilePath);
                    }
                     else if (command[0].Equals("GetExport"))
                    {                        
                        List<Question> quest = service.GetExport(DateTime.Parse(command[1]), DateTime.Parse(command[2]));
                        buffer = Serialize<List<Question>>(quest, serializedFilePath);
                    }
                     else if (command[0].Equals("GetAllQuestionsByUser"))
                    {                        
                        List<Question> quest = service.GetAllQuestionsByUser(int.Parse(command[1]));
                        buffer = Serialize<List<Question>>(quest, serializedFilePath);
                    }
                     else if (command[0].Equals("GetQByAnswer"))
                    {                        
                        Question quest = service.GetQByAnswer(int.Parse(command[1]));
                        buffer = Serialize<Question>(quest, serializedFilePath);
                    }
                    else if (command[0].Equals("UpdateQuestion"))
                    {      
                        Question question = Deserialize<Question>(serializedFilePath);                  
                        int quest = service.UpdateQuestion(question);
                        buffer = Serialize<int>(quest, serializedFilePath);
                    }
                    else if (command[0].Equals("GetUserById"))
                    {
                        User user = service.GetUserById(int.Parse(command[1]));
                        buffer = Serialize<User>(user, serializedFilePath);
                    }
                    else if (command[0].Equals("GetLastUserId"))
                    {
                        int user = service.GetLastUserId();
                        buffer = Serialize<int>(user, serializedFilePath);
                    }
                    else if (command[0].Equals("DeleteUserById"))
                    {
                        int user = service.DeleteUserById(int.Parse(command[1]));
                        buffer = Serialize<int>(user, serializedFilePath);
                    }
                    else if (command[0].Equals("InsertUser"))
                    {
                        User us = Deserialize<User>(serializedFilePath);
                        int user = service.InsertUser(us);
                        buffer = Serialize<int>(user, serializedFilePath);
                    }
                    else if (command[0].Equals("GetAllUsers"))
                    {
                        List<User> res = service.GetAllUsers();
                        buffer = Serialize<List<User>>(res, serializedFilePath);
                    }
                     else if (command[0].Equals("GetUserByName"))
                    {                        
                        User us = service.GetUserByName(command[1]);
                        buffer = Serialize<User>(us, serializedFilePath);
                    }
                    else if (command[0].Equals("GetQuestionAuthor"))
                    {                        
                        User us = service.GetQuestionAuthor(int.Parse(command[1]));
                        buffer = Serialize<User>(us, serializedFilePath);
                    }
                    else if (command[0].Equals("GetAnswerAuthor"))
                    {                        
                        User us = service.GetAnswerAuthor(int.Parse(command[1]));
                        buffer = Serialize<User>(us, serializedFilePath);
                    }
                    int nSentBytes = newClient.Send(buffer);
                    WriteLine("Response was sent");

                }
                catch(Exception ex)
                {
                    if (!ex.ToString().StartsWith("System.Net.Sockets."))
                    {
                        System.Console.WriteLine(ex);
                        WriteLine();
                    }
                    break;
                }
            }
        }
        public static void Main(string[] args)
        {
            databaseFile = "../data/new";
            connection = new SqliteConnection($"Data Source ={databaseFile}");
            connection.Open();

            IPAddress address = IPAddress.Loopback;
            int port = 3000;

            IPEndPoint ipEndPoint = new IPEndPoint(address, port);

            Socket serverSocket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(ipEndPoint);
                serverSocket.Listen(10);
                WriteLine("Server is listening on port: " + port);
                while (true)
                {
                    WriteLine("Waiting for a new client...");
                    Socket newClient = serverSocket.Accept();
                    WriteLine("Client has connected: " + newClient.RemoteEndPoint);

                    Thread thread = new Thread(StartNewThread);
                    thread.Start(newClient);
                }
            }
            catch
            {
                Error.WriteLine("Could not start a server on port: " + port);
            }

        }
            public static byte[] Serialize<T>(T data, string filePath)
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                System.IO.File.WriteAllText(filePath, "");
                System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath);
                ser.Serialize(writer, data);
                writer.Close();
                string text = System.IO.File.ReadAllText(filePath);
                byte[] bytes = Encoding.ASCII.GetBytes(text);


                return bytes;
            }
        public static T Deserialize<T>(string filePath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            StreamReader reader = new StreamReader(filePath);
            T value = (T)ser.Deserialize(reader);
            reader.Close();
            return value;
        }

    }
}