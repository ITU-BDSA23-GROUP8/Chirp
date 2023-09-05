using System.Reflection.Metadata;

class Cheep{
    string user; 
    string message; 
    DateTimeOffset timestamp;

    public Cheep(string user, string message, DateTimeOffset timestamp) {
        this.user = user; 
        this.message = message.Replace("\"", ""); //backslash is used to escape quotation mark
        this.timestamp = timestamp; 

    }

    public override string ToString()
    {
        // timestamp.toString is made with inspiration from https://www.shekhali.com/datetime-format-in-c-sharp-programming/
        return $"{user} @ {timestamp.ToString("MM/dd/yy HH:mm:ss")}: {message}";
    }




}