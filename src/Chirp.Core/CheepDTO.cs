namespace Chirp.Core;

/// <summary>
/// Contains compact Data Transfer Objects for Cheep and Author.
/// Used to transfer data to outer layer.
/// </summary>

public record CheepDTO(string AuthorName, string AuthorEmail, string Message, string Timestamp, int id, int likes);
public record AuthorDTO(string Name, string Email);
