namespace Chirp.CLI.Tests;


public class UnitTest1
{
    [Fact]
    public void TestPrintCheeps()
    {

        // Arrange

        long time = 1690891760;


        // Act 
        var actualTime = UserInterface.times(time).ToString("MM/dd/yy HH:mm:s");

        // Assert 

        Assert.Equal("08/01/23 14:09:20", actualTime);
        


    }
}