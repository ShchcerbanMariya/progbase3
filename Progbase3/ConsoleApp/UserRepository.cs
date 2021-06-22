using System;
using System.Collections.Generic;
using ConsoleApp;
using Microsoft.Data.Sqlite;
using System.Globalization;
namespace ConsoleApp
{
    public class UserRepository
    {
        private SqliteConnection connection;
        public UserRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public User GetById(int id)
        {
            connection.Open();
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
                user.isModerator = reader.GetString(3) == "moderator";
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
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * 
        FROM users 
        WHERE id = (SELECT max(id) FROM users)";
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
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM users WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            int nChanged = command.ExecuteNonQuery();
            return nChanged;
        }
        public int Insert(User user)
        {
            connection.Open();
            SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"INSERT INTO users (name, password, role)
            VALUES ($name, $password, $role);
            SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("$name", user.name);
            command.Parameters.AddWithValue("$password", user.password);
            command.Parameters.AddWithValue("$role", user.isModerator ? "moderator" : "user");

            long newId = (long)command.ExecuteScalar();
            return (int)newId;
        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();
            connection.Open();
            SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users";
            SqliteDataReader reader = command.ExecuteReader();
            List<User> postsToExport = new List<User>();
            while (reader.Read())
            {
                User user = new User();
                user.id = int.Parse(reader.GetString(0));
                user.name = reader.GetString(1);
                user.password = reader.GetString(2);
                user.isModerator = reader.GetString(3) == "moderator";

                postsToExport.Add(user);
            }
            reader.Close();
            return postsToExport;
        }
        public User GetByName(string name)
        {
            connection.Open();
            SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE name = $name";
            command.Parameters.AddWithValue("$name", name);

            SqliteDataReader reader = command.ExecuteReader();
            User user = new User();
            if (reader.Read())
            {
                user.id = int.Parse(reader.GetString(0));
                user.name = reader.GetString(1);
                user.password = reader.GetString(2);
                user.isModerator = reader.GetString(3) == "moderator";
            }
            else
            {
                user = null;
            }
            reader.Close();
            connection.Close();
            return user;
        }
        public User GetQuestionAuthor(int q_id)
        {
            connection.Open();
            SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"SELECT users.id, name, password, role FROM questions CROSS JOIN users WHERE questions.user_id = users.id AND questions.id = $q_id";
            command.Parameters.AddWithValue("$q_id", q_id);

            SqliteDataReader reader = command.ExecuteReader();
            User user = new User();
            if (reader.Read())
            {
                user.id = int.Parse(reader.GetString(0));
                user.name = reader.GetString(1);
                user.password = reader.GetString(2);
                user.isModerator = reader.GetString(3) == "moderator";
            }
            else
            {
                user = null;
            }
            reader.Close();
            return user;
        }
        public User GetAnswerAuthor(int ans_id)
        {
            connection.Open();
            SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"SELECT users.id, name, password, role FROM answers CROSS JOIN users WHERE answers.user_id = users.id AND answers.id = $ans_id";
            command.Parameters.AddWithValue("$ans_id", ans_id);

            SqliteDataReader reader = command.ExecuteReader();
            User user = new User();
            if (reader.Read())
            {
                user.id = int.Parse(reader.GetString(0));
                user.name = reader.GetString(1);
                user.password = reader.GetString(2);
                user.isModerator = reader.GetString(3) == "moderator";
            }
            else
            {
                user = null;
            }
            reader.Close();
            return user;
        }
    }
}