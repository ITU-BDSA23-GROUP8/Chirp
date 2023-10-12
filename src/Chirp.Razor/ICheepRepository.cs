

public interface ICheepRepository (){


    Task.IEnumerable<CheepDTO> GetCheeps{int pageSize = 32, int page = 0} 
     

}