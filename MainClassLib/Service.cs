using System;
using System.Collections.Generic;
using ServiceLib;
using Microsoft.Data.Sqlite;
namespace MainClassLib
{
    public class Service : IService
    {

        AnswerRepository answerRepo;
        QuestionsRepository questionRepo;
        UserRepository userRepo;
        SqliteConnection connection;
        string filePath;
        public Service(SqliteConnection connection, string filePath)
        {
            this.connection = connection;
            this.filePath = filePath;
            this.answerRepo = new AnswerRepository(connection, filePath);
            this.questionRepo = new QuestionsRepository(connection, filePath);
            this.userRepo = new UserRepository(connection, filePath);
        }
        public Answer GetAnswerById(int id)
        {
            return answerRepo.GetById(id);
        }
        public int DeleteAnswerById(int id)
        {
            return answerRepo.DeleteById(id);
        }
        public int InsertANswer(Answer answer)
        {
            return answerRepo.Insert(answer);
        }
        public int DeleteAllAnswersOfQuestion(int q_id)
        {
            return answerRepo.DeleteAllAnswersOfQuestion(q_id);
        }
        public int GetTotalAnswerPages()
        {
            return answerRepo.GetTotalPages();
        }
        public List<Answer> GetAllUserAns(int user_id)
        {
            return answerRepo.GetAllUserAns(user_id);
        }
        public List<Answer> GetByQuestionId(int q_id)
        {
            return answerRepo.GetByQuestionId(q_id);
        }
        public Answer GetMainAnswer(int q_id)
        {
            return answerRepo.GetMainAnswer(q_id);
        }
        public int UpdateAnswer(Answer answer)
        {
            return answerRepo.UpdateAnswer(answer);
        }
        public Question GetQuestionById(int id)
        {
            return questionRepo.GetById(id);
        }
        public List<Question> GetAllQuestions()
        {
            return questionRepo.GetAll();
        }
        public List<Question> GetAllContains(string text)
        {
            return questionRepo.GetAllContains(text);
        }
        public int DeleteQuestionById(int id)
        {
            return questionRepo.DeleteById(id);
        }
        // int DeleteById(int id);
        public int InsertQuestion(Question question)
        {
            return questionRepo.Insert(question);
        }
        public List<Question> GetExport(DateTime start, DateTime end)
        {
            return GetExport(start, end);
        }
        public List<Question> GetAllQuestionsByUser(int user_id)
        {
            return questionRepo.GetAllByUser(user_id);
        }
        public Question GetQByAnswer(int ans_id)
        {
            return questionRepo.GetByAnswer(ans_id);
        }
        public int UpdateQuestion(Question question)
        {
            return questionRepo.UpdateQuestion(question);
        }
        public User GetUserById(int id)
        {
            return userRepo.GetById(id);
        }
        public int GetLastUserId()
        {
            return userRepo.GetLastId();
        }
        public int DeleteUserById(int id)
        {
            return userRepo.DeleteById(id);
        }
        public int InsertUser(User user)
        {
            return userRepo.Insert(user);
        }
        public List<User> GetAllUsers()
        {
            return userRepo.GetAll();
        }
        public User GetUserByName(string name)
        {
            return userRepo.GetByName(name);
        }
        public User GetQuestionAuthor(int q_id)
        {
            return userRepo.GetQuestionAuthor(q_id);
        }
        public User GetAnswerAuthor(int ans_id)
        {
            return userRepo.GetAnswerAuthor(ans_id);
        }
    }
}