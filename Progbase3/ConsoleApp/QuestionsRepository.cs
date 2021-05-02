using System;
using System.IO;
using System.Data;
using ConsoleApp;
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
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM questions WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        SqliteDataReader reader = command.ExecuteReader();
        Question question = new Question();
        if (reader.Read())
        {
            question.id = int.Parse(reader.GetString(0));
            question.body = reader.GetString(1);
            question.user.id = int.Parse(reader.GetString(2));
            question.mainAnswer = int.Parse(reader.GetString(3));
            question.start = reader.GetDateTime(4);
            question.end = reader.GetDateTime(5);
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
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM questions WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        int nChanged = command.ExecuteNonQuery();
        return nChanged;
    }
    public int Insert(Question question)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"INSERT INTO questions (body, userAskId, mainAnswerId, start, end)
            VALUES ($body, $userAskId, $mainAnswerId, $start, $end);
            SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("$body", question.body);
        command.Parameters.AddWithValue("$userAskID", question.user.id);
        command.Parameters.AddWithValue("$mainAnswerId", question.mainAnswer);
        command.Parameters.AddWithValue("$start", question.start.ToString("o"));
        command.Parameters.AddWithValue("$question", question.end.ToString("o"));

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
        FROM users 
        WHERE id = (SELECT max(id) FROM users)";
        //command.Parameters.AddWithValue("$id", );
        SqliteDataReader reader = command.ExecuteReader();
        User user = new User();
        if (reader.Read())
        {
            user.id = int.Parse(reader.GetString(0));
        }
        else
        {
            user.id = 0;

        }
        reader.Close();
        return user.id;
    }
    public ListQuestions GetExport(DateTime valueX)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM questions";
        SqliteDataReader reader = command.ExecuteReader();
        ListQuestions postsToExport = new ListQuestions();
        while (reader.Read())
        {
            Question question = new Question();
            question.id = int.Parse(reader.GetString(0));
            question.body = reader.GetString(1);
            question.user.id = int.Parse(reader.GetString(2));
            question.mainAnswer = int.Parse(reader.GetString(3));
            question.start = reader.GetDateTime(4);
            question.end = reader.GetDateTime(5);

            postsToExport.AddQuestion(question);
        }
        reader.Close();
        return postsToExport;
    }
}