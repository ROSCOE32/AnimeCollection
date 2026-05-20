using AnimeCollection.Extensions;
using AnimeCollection.Models;
using Npgsql;

namespace AnimeCollection.Services;

public class FavoriService
{
    private readonly NpgsqlConnection _connection;

    public FavoriService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public bool IsFavorite(
        int userId,
        int personnageId)
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
            SELECT COUNT(*)
            FROM favoris
            WHERE userid = @userid
              AND personnageid = @personnageid
            """;

        command.AddParameter("@userid", userId);
        command.AddParameter("@personnageid", personnageId);

        var count = (long)command.ExecuteScalar()!;

        return count > 0;
    }

    public void Add(
        int userId,
        int personnageId)
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
            INSERT INTO favoris
            (
                userid,
                personnageid
            )
            VALUES
            (
                @userid,
                @personnageid
            )
            """;

        command.AddParameter("@userid", userId);
        command.AddParameter("@personnageid", personnageId);

        command.ExecuteNonQuery();
    }

    public void Remove(
        int userId,
        int personnageId)
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
            DELETE FROM favoris
            WHERE userid = @userid
              AND personnageid = @personnageid
            """;

        command.AddParameter("@userid", userId);
        command.AddParameter("@personnageid", personnageId);

        command.ExecuteNonQuery();
    }

    public List<Personnage> GetFavorites(
        int userId)
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
            FROM favoris f
            INNER JOIN personnages p
                ON f.personnageid = p.id
            INNER JOIN animes a
                ON p.animeid = a.id
            WHERE f.userid = @userid
            ORDER BY p.name
            """;

        command.AddParameter("@userid", userId);

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
}