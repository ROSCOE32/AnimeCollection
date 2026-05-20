using AnimeCollection.Extensions;
using AnimeCollection.Models;
using Npgsql;

namespace AnimeCollection.Services;

public class AnimeService
{
    private readonly NpgsqlConnection _connection;

    public AnimeService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public List<Anime> GetAll()
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
            SELECT Id, Title, Genre, ReleaseYear
            FROM Animes
            ORDER BY Title
            """;

        using var reader = command.ExecuteReader();

        var animes = new List<Anime>();

        while (reader.Read())
        {
            animes.Add(new Anime
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Genre = reader.GetString(2),
                ReleaseYear = reader.GetInt32(3)
            });
        }

        return animes;
    }

    public Anime? GetById(int id)
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
            SELECT Id, Title, Genre, ReleaseYear
            FROM Animes
            WHERE Id = @id
            """;

        command.AddParameter("@id", id);

        using var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new Anime
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            Genre = reader.GetString(2),
            ReleaseYear = reader.GetInt32(3)
        };
    }

    public void Create(Anime anime)
    {
        var command = _connection.CreateCommand();

        command.CommandText = """
            INSERT INTO "Animes" (Title, Genre, ReleaseYear)
            VALUES (@title, @genre, @releaseYear)
            """;

        command.AddParameter("@title", anime.Title);
        command.AddParameter("@genre", anime.Genre);
        command.AddParameter("@releaseYear", anime.ReleaseYear);

        command.ExecuteNonQuery();
    }

	public int CountAll()
	{
   		 var command = _connection.CreateCommand();

    	 command.CommandText = "SELECT COUNT(*) FROM animes";

    	var result = (long)command.ExecuteScalar()!;

    	return (int)result;
	}
}