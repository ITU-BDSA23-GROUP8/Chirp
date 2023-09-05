using System.Reflection.Metadata;

class Cheep{
    string user; 
    string message; 
    DateTimeOffset timestamp;

    public Cheep(string user, string message, DateTimeOffset timestamp) {
        this.user = user; 
        this.message = message;
        this.timestamp = timestamp; 

    }

    public override string ToString()
    {
        return $"{user} @ {timestamp.ToString("MM/dd/yy HH:mm:ss")}: {message}";
    }




}