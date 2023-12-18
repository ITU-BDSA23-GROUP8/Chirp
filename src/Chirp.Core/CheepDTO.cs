namespace Chirp.Core;

public record CheepDTO(string? AuthorName, string? AuthorEmail, string? Message, string? Timestamp);
public record AuthorDTO(string? Name, string? Email);