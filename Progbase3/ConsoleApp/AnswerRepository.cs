using System;
using System.Collections.Generic;
using ConsoleApp;
using Microsoft.Data.Sqlite;

public class AnswerRepository
{

    private SqliteConnection connection;
    public AnswerRepository(SqliteConnection connection)
    {
        this.connection = connection ;
    }

    public Answer GetById(int id)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM answers WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        SqliteDataReader reader = command.ExecuteReader();
        Answer answer = new Answer();
        if (reader.Read())
        {
            answer.id = int.Parse(reader.GetString(0));
            answer.body = reader.GetString(1);
            answer.user.id = int.Parse(reader.GetString(2));
            answer.mainAnswer = bool.Parse(reader.GetString(3));
            answer.question.id = int.Parse(reader.GetString(4));
            answer.time = reader.GetDateTime(5);
        }
        else
        {
            answer = null;

        }
        reader.Close();
        return answer;

    }
    public int DeleteById(int id)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM answers WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        int nChanged = command.ExecuteNonQuery();
        return nChanged;
    }
    public int Insert(Answer answer)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"INSERT INTO questions (body, userId, mainAnswer, questionId, time)
            VALUES ($body, $userId, $mainAnswer, $questionId, $time);
            SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("$body", answer.body);
        command.Parameters.AddWithValue("$userID", answer.user.id);
        command.Parameters.AddWithValue("$mainAnswerId", answer.mainAnswer);
        command.Parameters.AddWithValue("$start", answer.question.id);
        command.Parameters.AddWithValue("$question", answer.time.ToString("o"));

        long newId = (long)command.ExecuteScalar();
        return (int)newId;
    }
    public int GetTotalPages()
    {
        const int pageSize = 10;
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM answers";
        long count = (long)command.ExecuteScalar();
        int pages = (int)count / pageSize;
        return pages;
    }
    public List<Answer> GetExport(DateTime valueX)
    {
        connection.Open();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM answers";
        SqliteDataReader reader = command.ExecuteReader();
        List<Answer> postsToExport = new List<Answer>();
        while (reader.Read())
        {
            Answer answer = new Answer();
            answer.id = int.Parse(reader.GetString(0));
            answer.body = reader.GetString(1);
            answer.user.id = int.Parse(reader.GetString(2));
            answer.mainAnswer = bool.Parse(reader.GetString(3));
            answer.question.id = int.Parse(reader.GetString(4));
            answer.time = reader.GetDateTime(5);
            postsToExport.Add(answer);
        }
        reader.Close();
        return postsToExport;
    }
    public int GetLastId()
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * 
        FROM answers 
        WHERE id = (SELECT max(id) FROM answers)";
        //command.Parameters.AddWithValue("$id", );
        SqliteDataReader reader = command.ExecuteReader();
        Answer a = new Answer();
        if (reader.Read())
        {
            a.id = int.Parse(reader.GetString(0));
        }
        else
        {
            a.id = 0;

        }
        reader.Close();
        return a.id;
    }
    public List<Answer> GetAllUserAns(int user_id)
    {
        connection.Open();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * 
        FROM answers 
        WHERE user_id = $valueX";
        command.Parameters.AddWithValue("$valueX", user_id);
        SqliteDataReader reader = command.ExecuteReader();
        List<Answer> posts = new List<Answer>();
        while (reader.Read())
        {
            Answer answer = new Answer();
            answer.id = int.Parse(reader.GetString(0));
            answer.body = reader.GetString(1);
            answer.mainAnswer = bool.Parse(reader.GetString(3));
            //answer.question = int.Parse(reader.GetString(4));
            if(!reader.IsDBNull(5))
                if(reader.GetString(5) != "")
                    answer.time = reader.GetDateTime(5);
            posts.Add(answer);
        }
        reader.Close();
        connection.Close();
        return posts;
    }
    public List<Answer> GetByQuestionId(int q_id)
    {
        connection.Open();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * 
        FROM answers 
        WHERE question_id = $question_id";
        command.Parameters.AddWithValue("$question_id", q_id);
        SqliteDataReader reader = command.ExecuteReader();
        List<Answer> posts = new List<Answer>();
        while (reader.Read())
        {
            Answer answer = new Answer();
            answer.id = int.Parse(reader.GetString(0));
            answer.body = reader.GetString(1);
            answer.mainAnswer = bool.Parse(reader.GetString(3));
            //answer.question = int.Parse(reader.GetString(4));
            if(!reader.IsDBNull(5))
                answer.time = reader.GetDateTime(5);
            posts.Add(answer);
        }
        reader.Close();
        return posts;
    }
}