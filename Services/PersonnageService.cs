using AnimeCollection.Extensions;
using AnimeCollection.Models;
using Npgsql;

namespace AnimeCollection.Services;

public class PersonnageService
{
    private readonly NpgsqlConnection _connection;

    public PersonnageService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public List<Personnage> GetAll(string? search = null)
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
                              SELECT
                                  p.id,
                                  p.name,
                                  p.description,
                                  p.status,
                                  p.imagepath,
                                  p.animeid,
                                  a.title
                              FROM personnages p
                              INNER JOIN animes a ON p.animeid = a.id
                              WHERE @search::text IS NULL
                                 OR LOWER(p.name) LIKE LOWER(@search::text)
                                 OR LOWER(a.title) LIKE LOWER(@search::text)
                              ORDER BY p.name
                              """;

        command.AddParameter(
            "@search",
            string.IsNullOrWhiteSpace(search)
                ? null
                : $"%{search}%");

        using var reader = command.ExecuteReader();

        var personnages = new List<Personnage>();

        while (reader.Read())
        {
            personnages.Add(new Personnage
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Status = reader.GetString(3),
                ImagePath = reader.IsDBNull(4)
                    ? null
                    : reader.GetString(4),
                AnimeId = reader.GetInt32(5),
                AnimeTitle = reader.GetString(6)
            });
        }

        return personnages;
    }

    public Personnage? GetById(int id)
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
            SELECT
                p.id,
                p.name,
                p.description,
                p.status,
                p.imagepath,
                p.animeid,
                a.title
            FROM personnages p
            INNER JOIN animes a ON p.animeid = a.id
            WHERE p.id = @id
            """;

        command.AddParameter("@id", id);

        using var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new Personnage
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Description = reader.GetString(2),
            Status = reader.GetString(3),
            ImagePath = reader.IsDBNull(4)
                ? null
                : reader.GetString(4),
            AnimeId = reader.GetInt32(5),
            AnimeTitle = reader.GetString(6)
        };
    }

    public void Create(Personnage personnage)
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
            INSERT INTO personnages
            (
                name,
                description,
                status,
                imagepath,
                animeid
            )
            VALUES
            (
                @name,
                @description,
                @status,
                @imagepath,
                @animeid
            )
            """;

        command.AddParameter("@name", personnage.Name);
        command.AddParameter("@description", personnage.Description);
        command.AddParameter("@status", personnage.Status);
        command.AddParameter("@imagepath", personnage.ImagePath);
        command.AddParameter("@animeid", personnage.AnimeId);

        command.ExecuteNonQuery();
    }

    public bool Update(Personnage personnage)
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
            UPDATE personnages
            SET
                name = @name,
                description = @description,
                status = @status,
                imagepath = @imagepath,
                animeid = @animeid
            WHERE id = @id
            """;

        command.AddParameter("@id", personnage.Id);
        command.AddParameter("@name", personnage.Name);
        command.AddParameter("@description", personnage.Description);
        command.AddParameter("@status", personnage.Status);
        command.AddParameter("@imagepath", personnage.ImagePath);
        command.AddParameter("@animeid", personnage.AnimeId);

        var rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }

    public bool Delete(int id)
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
            DELETE FROM personnages
            WHERE id = @id
            """;

        command.AddParameter("@id", id);

        var rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }
}