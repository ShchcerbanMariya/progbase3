<<<<<<< HEAD
using System.IO;
using ConsoleApp;
using Microsoft.Data.Sqlite;
using System;
public static class GenerateData
{
    public static void GenerateDatabase(int usersCount, int answersCount, int questionsCount, DateTime start, DateTime end)
    {
        string dataPath = @"C:\Users\Myhasik\Desktop\progbase3\Progbase3\data\dataBase.db";
        SqliteConnection connection = new SqliteConnection($"Data Source={dataPath}");
        UserRepository userRepository = new UserRepository(connection);
        AnswerRepository answerRepository = new AnswerRepository(connection);
        QuestionsRepository questionRepository = new QuestionsRepository(connection);
        Answer[] answers = new Answer[answersCount];
        User[] users = new User[usersCount];
        Question[] questions = new Question[questionsCount];
        Random rand = new Random();
        for(int i = 0; i < usersCount; i++)
        {
            User user = new User();
            for(int j = 0; j < 8; j++)
            {
                user.name += (char)(rand.Next(97, 122));
                user.password += (char)(rand.Next(97, 122));
            }
            user.password = Authentication.getHash(user.password);
            user.isModerator = rand.Next(0, 5) == 0;
            user.id = userRepository.Insert(user);
            users[i] = user;
        }
        for(int i = 0; i < questionsCount; i++)
        {
            Question question = new Question();
            for(int j = 0; j < 8; j++)
            {
                question.title += (char)(rand.Next(97, 122));
            }
            for(int j = 0; j < 15; j++)
            {
                question.body += (char)(rand.Next(97, 122));
            }
            question.start = GenerateDate(start, end);
            if(rand.Next(0, 3) == 0)
            {
                question.end = GenerateDate(question.start, end);
            }
            question.user = users[rand.Next(0, users.Length)];
            question.id = questionRepository.Insert(question);
            questions[i] = question;
        }
        for(int i = 0; i < answersCount; i++)
        {
            Answer answer = new Answer();
            for (int j = 0; j < 8; j++)
            {
                answer.title += (char)(rand.Next(97, 122));
            }
            for (int j = 0; j < 15; j++)
            {
                answer.body += (char)(rand.Next(97, 122));
            }
            answer.user = users[rand.Next(0, users.Length)];
            int questionId = rand.Next(0, questions.Length);
            answer.question = questions[questionId];
            answer.time = GenerateDate(answer.question.start, answer.question.end == new DateTime() ? end : answer.question.end);
            answer.id = answerRepository.Insert(answer);
            questions[questionId].listAnswers.Add(answer);
        }
        Console.WriteLine("started4");
        for(int i = 0; i < questionsCount; i++)
        {
            if(questions[i].end != new DateTime())
            {
                if(questions[i].listAnswers.Count == 0)
                {
                    questions[i].end = new DateTime();
                }
                else
                {
                    Console.WriteLine(questions[i].listAnswers.Count);
                    questions[i].mainAnswer = questions[i].listAnswers[rand.Next(0, questions[i].listAnswers.Count)];
                }
            }
            questionRepository.UpdateQuestion(questions[i]);
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
=======
using System.IO;
using ConsoleApp;
using System.Collections.Generic;
using System.Linq;
using System;
namespace ConsoleApp
{

    class GenerateData
    {
        public User[] GenerateUsers(string nameFile, int numberUsers, Question[] questions, Answer[] answers)
        {
            if (File.Exists(nameFile))
            {
                List<string> parseUsers = File.ReadAllText(nameFile).Split('\n').ToList();
                User[] users = new User[numberUsers];
                Random rnd = new Random();
                for (int i = 0; i < numberUsers; i++)
                {
                    int questionsNumber = rnd.Next(1, (7 > questions.Length) ? questions.Length : 7);
                    int answersNumber = rnd.Next(1, (7 > answers.Length) ? answers.Length : 7);
                    List<Question> randomQuestions = questions.ToList();
                    List<Answer> randomAnswer = answers.ToList();
                    for (int j = 0; i < randomQuestions.Count - questionsNumber; j++)
                    {
                        int element = rnd.Next(0, questions.Length);
                        randomQuestions.RemoveAt(element);
                    }
                    for (int j = 0; i < randomAnswer.Count - answersNumber; j++)
                    {
                        int element = rnd.Next(0, answers.Length);
                        randomAnswer.RemoveAt(element);
                    }


                    int randUsId = rnd.Next(0, parseUsers.Count());
                    string[] currentuser = parseUsers[randUsId].Split(',');
                    string role;
                    bool rolebool;
                    if (rnd.Next(0, 5) == 0)
                    {
                        role = "moderator";
                        rolebool = true;
                    }
                    else
                    {
                        role = "user";
                        rolebool = false;
                    }
                    if (currentuser.Length == 2)
                    {
                        users[i] = new User(0, currentuser[0], currentuser[1], rolebool, randomQuestions, randomAnswer);
                        for (int j = 0; j < randomQuestions.Count(); j++)
                        {
                            for (int k = 0; k < questions.Length; k++)
                            {
                                if (questions[k].id == randomQuestions[j].id)
                                {
                                    questions[k].user = users[i];
                                    break;
                                }
                            }
                        }
                        for (int j = 0; j < randomAnswer.Count(); j++)
                        {
                            for (int k = 0; k < answers.Length; k++)
                            {
                                if (answers[k].id == randomAnswer[j].id)
                                {
                                    answers[k].user = users[i];
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                return users;
            }
            else
            {
                throw new Exception();
            }
        }
        /*    public List<Question> GenerateQuestionsAndAnswers( int questionsNum, DateTime firstLim, DateTime secondLim)
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
>>>>>>> 7cba30f923206795fb12ece32b2ea17fe5f6c188
}