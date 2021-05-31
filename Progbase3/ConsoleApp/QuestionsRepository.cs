using System;
using ConsoleApp;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
public class QuestionsRepository
{

    private SqliteConnection connection;
    public QuestionsRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }
    public Question GetById(int id)
    {
        connection.Open();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM questions WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        User user = new User();
        SqliteDataReader reader = command.ExecuteReader();
        Question question = new Question();
        if (reader.Read())
        {
            question.id = int.Parse(reader.GetString(0));
            question.body = reader.GetString(1);
            question.start = Convert.ToDateTime(reader.GetDateTime(3));
            question.end = Convert.ToDateTime(reader.GetDateTime(4));
        }
        else
        {
            question = null;

        }
        reader.Close();
        return question;

    }
    public int DeleteById(int id)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM questions WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        int nChanged = command.ExecuteNonQuery();
        return nChanged;
    }
    public int Insert(Question question)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"INSERT INTO questions (body, user_id, startData, endData)
            VALUES ($body, $user_id, $startData, $endData);
            SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("$body", question.body);
        command.Parameters.AddWithValue("$user_id", question.user.id);
        //command.Parameters.AddWithValue("$mainAnswerId", question.mainAnswer);
        command.Parameters.AddWithValue("$startData", question.start.ToString("o"));
        command.Parameters.AddWithValue("$endData", question.end.ToString("o"));

        long newId = (long)command.ExecuteScalar();
        return (int)newId;
    }
    public int GetTotalPages()
    {
        const int pageSize = 10;
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM questions";
        long count = (long)command.ExecuteScalar();
        int pages = (int)count / pageSize;
        return pages;
    }
    public int GetLastId()
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * 
        FROM questions 
        WHERE id = (SELECT max(id) FROM questions)";
        //command.Parameters.AddWithValue("$id", );
        SqliteDataReader reader = command.ExecuteReader();
        Question q = new Question();
        if (reader.Read())
        {
            q.id = int.Parse(reader.GetString(0));
        }
        else
        {
            q.id = 0;

        }
        reader.Close();
        return q.id;
    }
    public List<Question> GetExport(DateTime valueX)
    {
        connection.Open();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM questions";
        SqliteDataReader reader = command.ExecuteReader();
        List<Question> postsToExport = new List<Question>();
        while (reader.Read())
        {
            Question question = new Question();
            question.id = int.Parse(reader.GetString(0));
            question.body = reader.GetString(1);
            question.user.id = int.Parse(reader.GetString(2));
            //question.mainAnswer = int.Parse(reader.GetString(3));
            question.start = reader.GetDateTime(4);
            question.end = reader.GetDateTime(5);

            postsToExport.Add(question);
        }
        reader.Close();
        return postsToExport;
    }
    public List<Question> GetAllByUser(int user_id)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * 
        FROM questions 
        WHERE user_id = $user_id";
        command.Parameters.AddWithValue("$user_id", user_id);
        SqliteDataReader reader = command.ExecuteReader();
        List<Question> posts = new List<Question>();
        while (reader.Read())
        {
            Question question = new Question();
            question.id = int.Parse(reader.GetString(0));
            question.body = reader.GetString(1);
            question.start = reader.GetDateTime(3);
            question.end = reader.GetDateTime(4);
            posts.Add(question);
        }
        reader.Close();
        connection.Close();
        return posts;
    }
    public Question GetByAnswer(int ans_id)
    {
        connection.Open();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT questions.id, questions.body, questions.startData, questions.endData 
        FROM questions CROSS JOIN answers 
        WHERE questions.id = answers.question_id AND answers.id = $id";
        command.Parameters.AddWithValue("$id", ans_id);
        User user = new User();
        SqliteDataReader reader = command.ExecuteReader();
        Question question = new Question();
        if (reader.Read())
        {
            question.id = int.Parse(reader.GetString(0));
            question.body = reader.GetString(1);
            question.start = reader.GetDateTime(2);
            question.end = reader.GetDateTime(3);
        }
        else
        {
            question = null;

        }
        reader.Close();
        return question;
    }
    public int UpdateQuestion(Question question)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"UPDATE questions SET body = $body WHERE id = $id;
        SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("$body", question.body);
        command.Parameters.AddWithValue("$id", question.id);
        return (int)(long)command.ExecuteScalar();
    }
}