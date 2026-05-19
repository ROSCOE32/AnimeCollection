using AnimeCollection.Extensions;
using AnimeCollection.Models;
using Npgsql;

namespace AnimeCollection.Services;

public class UserService
{
    private readonly NpgsqlConnection _connection;

    public UserService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public User? GetByEmail(string email)
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
                              SELECT
                                  id,
                                  name,
                                  email,
                                  passwordhash,
                                  role
                              FROM users
                              WHERE email = @email
                              """;

        command.AddParameter("@email", email);

        using var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new User
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Email = reader.GetString(2),
            PasswordHash = reader.GetString(3),
            Role = reader.GetString(4)
        };
    }

    public void Create(User user)
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
                              INSERT INTO users
                              (
                                  name,
                                  email,
                                  passwordhash,
                                  role
                              )
                              VALUES
                              (
                                  @name,
                                  @email,
                                  @passwordhash,
                                  @role
                              )
                              """;

        command.AddParameter("@name", user.Name);
        command.AddParameter("@email", user.Email);
        command.AddParameter("@passwordhash", user.PasswordHash);
        command.AddParameter("@role", user.Role);

        command.ExecuteNonQuery();
    }
}