using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using ConsoleApp;
using System.IO;

namespace ConsoleApp
{
    public static class ImportExport
    {
        public static void Import(QuestionsRepository questionsRepository, AnswerRepository answerRepository,string importPath)
        {
            if(Path.GetExtension(importPath) == ".xml")
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<Question>));
                List<Question> questions = (List<Question>)ser.Deserialize(new StreamReader(importPath));
                for (int i = 0; i < questions.Count; i++)
                {
                    if(questions[i].user != null && questions[i].mainAnswer.user != null) 
                    {
                        answerRepository.Insert(questions[i].mainAnswer);
                        questionsRepository.Insert(questions[i]);
                    }
                }

            }
            else
            {
                throw new Exception("Wrong format");
            }
        }
        public static void Export(DateTime start, DateTime end, string exportPath, QuestionsRepository questionsRepository)
        {
            List<Question> export = questionsRepository.GetExport(start, end);
            Console.WriteLine(export.Count);
            XmlSerializer ser = new XmlSerializer(typeof(List<Question>));
            ser.Serialize(new StreamWriter(exportPath), export);
        }
    }
}
