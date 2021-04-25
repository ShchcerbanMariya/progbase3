using System;
using System.IO;
using System.Data;
using ConsoleApp;
using Microsoft.Data.Sqlite;

public class QuestionRepository
{

    private SqliteConnection connection;
    public QuestionRepository(SqliteConnection connection)
    {
        this.connection = connection;
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
            answer.userID = int.Parse(reader.GetString(2));
            answer.mainAnswer = bool.Parse(reader.GetString(3));
            answer.questionId = int.Parse(reader.GetString(4));
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
        command.Parameters.AddWithValue("$userID", answer.userID);
        command.Parameters.AddWithValue("$mainAnswerId", answer.mainAnswer);
        command.Parameters.AddWithValue("$start", answer.questionId);
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
    public ListAnswers GetExport(DateTime valueX)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM answers";
        SqliteDataReader reader = command.ExecuteReader();
        ListAnswers postsToExport = new ListAnswers();
        while (reader.Read())
        {
            Answer answer = new Answer();
            answer.id = int.Parse(reader.GetString(0));
            answer.body = reader.GetString(1);
            answer.userID = int.Parse(reader.GetString(2));
            answer.mainAnswer = bool.Parse(reader.GetString(3));
            answer.questionId = int.Parse(reader.GetString(4));
            answer.time = reader.GetDateTime(5);

            postsToExport.AddAnswer(answer);
        }
        reader.Close();
        return postsToExport;
    }
}