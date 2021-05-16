using System;
using System.IO;
using System.Data;
using ConsoleApp;
using Microsoft.Data.Sqlite;
public class UserRepository
{

    private SqliteConnection connection;
    public UserRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public User GetById(int id)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM users WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        SqliteDataReader reader = command.ExecuteReader();
        User user = new User();
        if (reader.Read())
        {
            user.id = int.Parse(reader.GetString(0));
            user.name = reader.GetString(1);
            user.password = reader.GetString(2);
            user.role = reader.GetString(3);
        }
        else
        {
            user = null;
        }
        reader.Close();
        return user;

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
    public int DeleteById(int id)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM users WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        int nChanged = command.ExecuteNonQuery();
        return nChanged;
    }
    public int Insert(User user)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"INSERT INTO users (name, password, role)
            VALUES ($name, $password, $role);
            SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("$name", user.name);
        command.Parameters.AddWithValue("$password", user.password);
        command.Parameters.AddWithValue("$role", user.role);

        long newId = (long)command.ExecuteScalar();
        return (int)newId;
    }
    public int GetTotalPages()
    {
        const int pageSize = 10;
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM users";
        long count = (long)command.ExecuteScalar();
        int pages = (int)count / pageSize;
        return pages;
    }
    public ListUsers GetExport()
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM users";
        SqliteDataReader reader = command.ExecuteReader();
        ListUsers postsToExport = new ListUsers();
        while (reader.Read())
        {
            User user = new User();
            user.id = int.Parse(reader.GetString(0));
            user.name = reader.GetString(1);
            user.password = reader.GetString(2);
            user.role = reader.GetString(3);

            postsToExport.AddUser(user);
        }
        reader.Close();
        return postsToExport;
    }
   
    /* static User ConvertToUser(string s)
     {
         string[] array = s.Split(',');
         if (array.Length == 6)
         {
             int id = int.Parse(array[0]);
             string name = array[1];
             string password = array[2];
             string role = array[3];
             Random r = new Random();
             string databaseFileName = "/home/mariya/Desktop/projects/progbase3/Progbase3/data/dataBase";
             SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
             connection.Open();
             QuestionsRepository repository = new QuestionsRepository(connection);
             int lastId = repository.GetLastId();

             int size = r.Next(0, 7);
             Question[] arrayofq= new Question[size];
             for(int i =0; i<size; i++)
             {
               int num = r.Next(1, lastId + 1);
               arrayofq[i] = repository.GetById(num);
             }

             User us = new User(id, name, password, role, arrayofq);
             return us;
         }
         return new User();
     }*/
}