using System.IO;
using Microsoft.Data.Sqlite;
using System;
using MainClassLib;
using ConsoleApp;
using ServiceLib;
public static class GenerateData
{
    public static void Main(string[] args)
    {}
    public static void GenerateDatabase(int usersCount, int answersCount, int questionsCount, DateTime start, DateTime end)
    {
        string dataPath = @"../data/new";
        SqliteConnection connection = new SqliteConnection($"Data Source={dataPath}");
        UserRepository userRepository = new UserRepository(connection, dataPath);
        AnswerRepository answerRepository = new AnswerRepository(connection, dataPath);
        QuestionsRepository questionRepository = new QuestionsRepository(connection, dataPath);
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
}