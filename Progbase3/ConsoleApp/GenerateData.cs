using System.IO;
using ConsoleApp;
using System.Collections.Generic;
using System.Linq;
using System;
class GenerateData
{
    //public User[] GenerateUsers(string nameFile, int numberUsers, Question[] questions, Answer[] answers)
    //{
    //    if (File.Exists(nameFile))
    //    {
    //        List<string> parseUsers = File.ReadAllText(nameFile).Split('\n').ToList();
    //        User[] users = new User[numberUsers];
    //        Random rnd = new Random();
    //        for (int i = 0; i < numberUsers; i++)
    //        {
    //            int questionsNumber = rnd.Next(1, (7 > questions.Length) ? questions.Length : 7);
    //            int answersNumber = rnd.Next(1, (7 > answers.Length) ? answers.Length : 7);
    //            List<Question> randomQuestions = questions.ToList();
    //            List<Answer> randomAnswer = answers.ToList();
    //            for (int j = 0; i < randomQuestions.Count - questionsNumber; j++)
    //            {
    //                int element = rnd.Next(0, questions.Length);
    //                randomQuestions.RemoveAt(element);
    //            }
    //            for (int j = 0; i < randomAnswer.Count - answersNumber; j++)
    //            {
    //                int element = rnd.Next(0, answers.Length);
    //                randomAnswer.RemoveAt(element);
    //            }


    //            int randUsId = rnd.Next(0, parseUsers.Count());
    //            string[] currentuser = parseUsers[randUsId].Split(',');
    //            string role;
    //            if (rnd.Next(0, 5) == 0)
    //            {
    //                role = "moderator";
    //            }
    //            else
    //            {
    //                role = "user";
    //            }
    //            if (currentuser.Length == 2)
    //            {
    //                users[i] = new User(0, currentuser[0], currentuser[1], role, randomQuestions, randomAnswer);
    //                for(int j = 0; j < randomQuestions.Count(); j++)
    //                {
    //                    for(int k = 0; k < questions.Length; k++)
    //                    {
    //                        if(questions[k].id == randomQuestions[j].id)
    //                        {
    //                            questions[k].user = users[i];
    //                            break;
    //                        }
    //                    }
    //                }
    //                for(int j = 0; j < randomAnswer.Count(); j++)
    //                {
    //                    for(int k = 0; k < answers.Length; k++)
    //                    {
    //                        if(answers[k].id == randomAnswer[j].id)
    //                        {
    //                            answers[k].user = users[i];
    //                            break;
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                throw new Exception();
    //            }
    //        }
    //        return users;
    //    }
    //    else
    //    {
    //        throw new Exception();
    //    }
    //}
    /*public List<Question> GenerateQuestionsAndAnswers( int questionsNum, DateTime firstLim, DateTime secondLim)
    {
        if (secondLim < firstLim)
        {
            throw new Exception();
        }
        List<Question> quest = new List<Question>();
        Random rnd = new Random();
        for (int i = 0; i < questionsNum; i++)
        {
            int qLength = rnd.Next(20, 40);
            string text = "";
            for(int j = 0; j < )
            if (currentQuestion.Length == 1)
            {
                DateTime generateDate = GenerateDate(firstLim, secondLim);
                quest[i] = new Question(i+1, currentQuestion[0], 0, generateDate, GenerateDate(generateDate, secondLim));
            }
            else
            {
                throw new Exception();
            }
        }
        return quest;
    }*/
    public Answer[] GenerateAnswersForQuestion(string nameFile, int answersNum, Question[] questions)
    {
        if (File.Exists(nameFile))
        {
            List<string> parseAnswers = File.ReadAllText(nameFile).Split('\n').ToList();
            Answer[] answers = new Answer[answersNum];
            Random rnd = new Random();
            for (int i = 0; i < answersNum; i++)
            {
                int randAnswerId = rnd.Next(0, parseAnswers.Count());
                int randQId = rnd.Next(0, questions.Length);
                string stringAnswer = parseAnswers[randAnswerId];
                string[] currentAnswer = stringAnswer.Split(',');
                parseAnswers.Remove(stringAnswer);
                if (currentAnswer.Length == 1)
                {
                    answers[i] = new Answer(0, currentAnswer[0], false, GenerateDate(questions[randQId].start, questions[randQId].end));
                    questions[randQId].listAnswers.Add(answers[i]);
                }
                else
                {
                    throw new Exception();
                }
            }
            return answers;
        }
        else
        {
            throw new Exception();
        }
    }
    static DateTime GenerateDate(DateTime firstLim, DateTime secondLim)
    {
        Random rand = new Random();
        double limits = (secondLim - firstLim).TotalDays;
        double addDays = rand.Next(1, (int)limits);
        DateTime newDate = firstLim.AddDays(addDays);
        return newDate;
    }
}