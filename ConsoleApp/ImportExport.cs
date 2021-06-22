using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using ConsoleApp;
using System.IO;
using ServiceLib;

namespace ConsoleApp
{
    public static class ImportExport
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
        //public static void Import(string importPath)


        public static void Export(DateTime start, DateTime end, string exportPath)
        {
            RemoteService.RemoteServiceCommand("GetExport$" + start + "$" + end);
            List<Question> export = //questionsRepository.GetExport(start, end);
            Deserialize<List<Question>>(serializedFilePath);
            Console.WriteLine(export.Count);
            XmlSerializer ser = new XmlSerializer(typeof(List<Question>));
            ser.Serialize(new StreamWriter(exportPath), export);
        }
        public static void Import(string importPath)
        {
            if (Path.GetExtension(importPath) == ".xml")
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<Question>));
                List<Question> questions = (List<Question>)ser.Deserialize(new StreamReader(importPath));
                for (int i = 0; i < questions.Count; i++)
                {
                    if (questions[i].user != null && questions[i].mainAnswer.user != null)
                    {
                        Serialize<Answer>(questions[i].mainAnswer, serializedFilePath);
                        RemoteService.RemoteServiceCommand("InsertANswer$" + questions[i].mainAnswer);
                        Deserialize<int>(serializedFilePath);
                        //answerRepository.Insert(questions[i].mainAnswer);
                        RemoteService.RemoteServiceCommand("InsertQuestion$" + questions[i]);
                        Deserialize<int>(serializedFilePath);
                        // questionsRepository.Insert(questions[i]);
                    }
                }

            }
            else throw new Exception("Wrong format");
        }

    }


}
