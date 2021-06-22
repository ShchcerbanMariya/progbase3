using System.Collections.Generic;
using System;
namespace ServiceLib
{
    public interface IService
    {
        Answer GetAnswerById(int id);
        int DeleteAnswerById(int id);
        int InsertANswer(Answer answer);
        int DeleteAllAnswersOfQuestion(int q_id);
        int GetTotalAnswerPages();
        List<Answer> GetAllUserAns(int user_id);
        List<Answer> GetByQuestionId(int q_id);
        Answer GetMainAnswer(int q_id);
        int UpdateAnswer(Answer answer);
        Question GetQuestionById(int id);
        List<Question> GetAllQuestions();
        List<Question> GetAllContains(string text);
        int DeleteQuestionById(int id);
       // int DeleteById(int id);
        int InsertQuestion(Question question);
        List<Question> GetExport(DateTime start, DateTime end);
        List<Question> GetAllQuestionsByUser(int user_id);
        Question GetQByAnswer(int ans_id);
        int UpdateQuestion(Question question);
        User GetUserById(int id);
        int GetLastUserId();
        int DeleteUserById(int id);
        int InsertUser(User user);
        List<User> GetAllUsers();
        User GetUserByName(string name);
        User GetQuestionAuthor(int q_id);
        User GetAnswerAuthor(int ans_id);

    }
}