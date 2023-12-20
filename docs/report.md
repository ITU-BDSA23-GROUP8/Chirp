---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group 8
author:
- "Annika Jensby Lütken <alyt@itu.dk>"
- "Astrid Emilie Bagge-Kjær <astb@itu.dk>"
- "Clara Augusta Sønderborg <auso@itu.dk>"
- "Julia Sjoukje Klompmaker <jukl@itu.dk>"
- "Sarah Schalls Vestergren <sscv@itu.dk>"
numbersections: true
---


# Design and Architecture of _Chirp!_

## Domain model
![Illustration of the _Chirp!_ data model as UML class diagram.](images/DomainModel.drawio.png)

The domain model that represents our Chirp! application is shown in the given diagram. As pictured there are three main concepts; Author, Cheep and Like. 
Like
Like holds an Author- and CheepId as well as the Author that liked the Cheep and which Cheep was liked. 

### Cheep
The Cheep consists of an id, a timestamp, an author id and it holds the message that is posted to the Chirp! website. Furthermore, it takes an Author and a number of Likes. Here it is vital that a Cheep can only have one author, but have many likes from different users of Chirp!. 

### Author
Author represents the user of Chirp!. The author consists of the Cheeps they have written, the likes they have given and lists of Followers and Following, that each represents other Authors the user follows, and Authors that follow the user. 
The Author inherits from IdentityUser, which is a part of AspNetCore. IdentityUser holds different fields that Author uses; Id, UserName and Email. 

### ASP.NET Core Identity
Chirp uses ASP.NET Core Identity for the login functionality and management of the users of Chirp!. It also supports authentication and authorization. In this Chirp!-implementation GitHub is used as a third party to log in. GitHub checks the username and password and authenticates to an existing GitHub-user. 
([See ASP.NET Core Identity documentation](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-8.0&tabs=visual-studio))

We chose to use ASP.NET Core Identity because, while there is some auto generated code it gives freedom to shape the code to fit our needs. This was prevalent e.g. when making the Author, because while we extended IdentityUser, we were also able to customize our Author-type to fit the needs of Chirp!. ([See ASP.NET Core Identity customization documentation](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-8.0))


## Architecture — In the small

![Illustration of the _Chirp!_ architecture.](images/OnionAndDomainModel1.png)

The given diagram represents an Onion Architecture. The layers are divided into ‘Core’,  ‘Infrastructure’ and ‘Web’. Each of our classes are represented in their given layer, according to their functionality in the program. 

The ‘Core’ layer reflects our application logic, and holds the repository interfaces and DTO’s (Data Transfer Objects).

The  ‘Infrastructure’ layer follows the business rules set in our Core-layer. It holds our data models such as ‘Like’, ‘Cheep’ and ‘Author’, with implementations of their given repositories. The repositories contain logic of how our data models behave with each other and according to the user input. Here it is important to note that our class ChirpContext acts as a gateway between the Core-layer and the Web-layer. 

The ‘Web’ layer, reflects the Framework of the Chirp! Application, and hereby holds Program.cs, Razor Pages, etc.


## Architecture of deployed application

![Illustration of the _Chirp!_ deployed application architecture](images/DeployedApplicationArchitecture.png)

The illustration displays that we are using a client/server architecture in our program, with the Azure Server connecting to an Azure SQL Server Database. 


## User activities
![Illustration of the _Chirp!_ user activity](images/UserActivity.drawio.png)

The activity diagram illustrates how a user can journey through our Chirp! application. It shows what an anonymous user can do within the application, where they can navigate to three different pages. If the user is logged in, they can see a public timeline, a user timeline for another user or their own timeline. The user can like and unlike cheeps from all mentioned pages, as well as follow and unfollow other users. But it is only from the public timeline that a user can post a cheep. 

The user can end the journey by ‘Forget me’ from the Profile page or anytime by Logout page. 


## Sequence of functionality/calls trough _Chirp!_

![Illustration of the _Chirp!_ calls/functionality sequence](images/SeuqenceDiagram.png)


The sequence diagram illustrates different calls through our Chirp! application. There are 4 lifelines: ‘Client’, ‘Chirp.Web’, ‘Chirp.Infrastructure’ and ‘Database’. 

The client can send HTTP calls to Chirp.Web which then, on behalf of the HTTP call, makes a C# call to Chirp.Infrastructure. Infrastructure is able to communicate with the database through SQL queries which then generates the response to the client. This shows how the different subsystems of our program communicate with each other based on the calls from the client. 
Notice the client is not authenticated at the beginning of the sequence but there will still be sent a HTTP get request to get all cheeps from the database as these are visible even for an unauthorized user. 

The diagram also illustrates the process of getting authenticated by Github. The authentication in Oauth with GitHub sequence is not shown in the diagram. But in short, Chirp! uses Oauth/Github to authenticate users via GitHub API. 

# Process

## Build, test, release, and deployment

Illustrate with a UML activity diagram how your _Chirp!_ applications are build, tested, released, and deployed.
That is, illustrate the flow of activities in your respective GitHub Actions workflows.

Describe the illustration briefly, i.e., how your application is built, tested, released, and deployed.

## Team work

Show a screenshot of your project board right before hand-in.
Briefly describe which tasks are still unresolved, i.e., which features are missing from your applications or which functionality is incomplete.

Briefly describe and illustrate the flow of activities that happen from the new creation of an issue (task description), over development, etc. until a feature is finally merged into the `main` branch of your repository.

### How to make _Chirp!_ work locally

There has to be some documentation on how to come from cloning your project to a running system.
That is, Rasmus or Helge have to know precisely what to do in which order.
Likely, it is best to describe how we clone your project, which commands we have to execute, and what we are supposed to see then.

***Software needed***: 
- VSCODE
- .NET 7.0
- Docker
- A browser that is *not* Google Chrome

<br>

**1. Cloning the project**
1. Go to the Github repository page for Group 8: 
     - https://github.com/ITU-BDSA23-GROUP8/Chirp
2. Press on the green button '*<> Code*'  and copy the HTTPS address
3. Open a new window in VSCODE
4. Press '*Clone Git Repository*' and paste in the address from **step 2**. 
5. Confirm you are able to see the folders; all the code -  including tests etc. 
6. In the /Chirp.Web folder, make a new file with the name: 

	*"appsettings.Development.json"*
7. In this file, paste the following: 
```

		{
			"DetailedErrors": true,
			"Logging": {
				"LogLevel": {
					"Default": "Information",
					"Microsoft.AspNetCore": "Warning"
				}
			},
			"ConnectionStrings": {
				"SqlServer": "Server=127.0.0.1,1433;Database=Master;User Id=SA;Password=<YourStrong@Passw0rd> ;TrustServerCertificate=True"
			},
			"AllowedHosts": "*"
		}	

```

		
<br>

**2. Starting the database**
1. Open the Docker Desktop program
2. In VSCODE, start a new terminal from /Chirp folder 
3. To start the Database, enter all of the following into the terminal (can be done in one command): 

       docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong@Passw0rd>" \
		-p 1433:1433 --name sql1 --hostname sql1 \ 
		-d \ 
		mcr.microsoft.com/mssql/server:2022-latest

<br>

**3. Starting the server**
1. In VSCODE, start a new terminal from the /ChirpWeb folder 
2. To start the server on your local machine, type in the terminal: 

        dotnet run
3. In the terminal output, find the localhost link and copy the address: 
![ScreenshotOfTerminal](images/localhostMarked.png)
4. Open a new window in Firefox and paste in the address from **step 3**. 
		

**4. Expected Result**
1. You should arrive at the main page of the Chirp! Application 

![ScreenshotOfChirpWeb](images/ChirpStartPage.png)

## How to run test suite locally


Our test suite is completed of 20 tests, of which are either Unit-, Integration- or End2End-tests. 3 of the 20 tests are made with Playwright. 

The **unit tests** mainly test that our SQL database and Chirp.Core work as expected and create the corresponding object DTO's. 

The **integration tests** check the website displays the correct information, and that the client is on the correct page (Public timeline / Private timeline), with the relevant info for this page. 

The **end2end tests**, tests the program's overall functionality from start to end, including login and authorization. 

As a note, we are aware that the Playwright tests currently tests directly upon the PROD Server website (meaning, test cheeps etc are actually publicized, although then deleted again in the tests). 
Ideally, one would have a seperate environment 'TEST', in which these end2end tests are actualized. 

***Software needed***: 
- VSCODE
- .NET 7.0
- Playwright

**1. Running the tests**
1. Confirm you have Playwright installed and open. 
2. In VSCODE, start a new terminal from the /Chirp folder 
3. To run the first part of the test suite (except from Playwright tests), type in the terminal: 
		
		dotnet test

4. **Expected result *1st Part*** should look alike, with 17 passed tests.: 

	![ScreenshotOfTerminal](images/FirstPartOfTests.png)

5. Now, open a second terminal, from the /test/PlaywrightTests folder
6. To run the second part of the test suite (non-Playwright tests), type in this terminal: 

		dotnet test

4. **Expected result *2nd Part*** should look alike, with 3 passed tests.: (OBS: You may have to manually press green button 'Authorize' at one point during the testing).

	![ScreenshotOfTerminal](images/3PassedPlaywrightTests.png)

# Ethics

## License
We use an MIT License for software. We chose this license as it is very simple and permissive. It grants permission to modifying, distributing, private and commercial use. As there is no warranty and liability, the copyright holders are not responsible for how the software is used. 

## LLMs, ChatGPT, CoPilot, and others
We did not make use of any LLMs in our project. 
