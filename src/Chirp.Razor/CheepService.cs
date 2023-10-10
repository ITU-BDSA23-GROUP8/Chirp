using System.Data;
using Microsoft.Data.Sqlite;
using System.Reflection;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}


public class CheepService : ICheepService
{
    DBFacade dBFacade = new();

     //amountOfCheeps reads the number of cheeps when showing 32 cheeps pr page
    public int amountOfCheeps(int page){
        int amountOfCheeps = (page == 1 ? 0 : 32 * (page-1));
        return amountOfCheeps;
    }

    public List<CheepViewModel> GetCheeps(int page)
    {
        // we have implemented a limit and offset of our amountOfCheeps, to calculate the correct cheeps to show on the page
        var sqlQuery = $@"SELECT username, text, pub_date 
                        FROM message 
                        JOIN user ON author_id = user_id 
                        ORDER by message.pub_date desc

                        LIMIT 32

                        OFFSET { amountOfCheeps(page)}";

       

        return dBFacade.GetCheeps(sqlQuery);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string user, int page)
    {          
        var sqlQuery = $@"SELECT username, text, pub_date 
                        FROM message 
                        JOIN user ON author_id = user_id 
                        WHERE username = @user 
                        ORDER by message.pub_date desc

                        LIMIT 32
                        
                        OFFSET { amountOfCheeps(page)}";
                        

        return dBFacade.GetCheepsFromAuthor(sqlQuery, user);
    }

   

}
