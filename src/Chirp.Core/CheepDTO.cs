namespace Chirp.Core;

public record CheepDTO(string AuthorName, string AuthorEmail, string Message, string Timestamp, int id, int likes);
public record LikeDTO(int cheepid, int authorid);
public record AuthorDTO(string Name, string Email);
