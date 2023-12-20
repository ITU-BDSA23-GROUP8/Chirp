---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group 8
author:
- "Annika Jensby Lütken <alyt@itu.dk>"
- "Astrid Emilie Bagge-Kjær <astb@itu.dk>"
- "Clara Augusta Sønderborg <auso@itu.dk>"
- "Julie Sjoukje Klompmaker <jukl@itu.dk>"
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

Illustrate the organization of your code base.
That is, illustrate which layers exist in your (onion) architecture.
Make sure to illustrate which part of your code is residing in which layer.

## Architecture of deployed application

Illustrate the architecture of your deployed application.
Remember, you developed a client-server application.
Illustrate the server component and to where it is deployed, illustrate a client component, and show how these communicate with each other.

**OBS**: In case you ran out of credits for hosting an Azure SQL database and you switched back to deploying an application with in-process SQLite database, then do the following:

- Under this section, provide two diagrams, one that shows how _Chirp!_ was deployed with hosted database and one that shows how it is now again with SQLite.
- Under this section, provide a brief description of the reason for switching again to SQLite as database.
- In that description, provide a link to the commit hash in your GitHub repository that points to the latest version of your _Chirp!_ application with hosted database (we look at the entire history of your project, so we see that it was there at some point).

## User activities

Illustrate typical scenarios of a user journey through your _Chirp!_ application.
That is, start illustrating the first page that is presented to a non-authorized user, illustrate what a non-authorized user can do with your _Chirp!_ application, and finally illustrate what a user can do after authentication.

Make sure that the illustrations are in line with the actual behavior of your application.

## Sequence of functionality/calls trough _Chirp!_

With a UML sequence diagram, illustrate the flow of messages and data through your _Chirp!_ application.
Start with an HTTP request that is send by an unauthorized user to the root endpoint of your application and end with the completely rendered web-page that is returned to the user.

Make sure that your illustration is complete.
That is, likely for many of you there will be different kinds of "calls" and responses.
Some HTTP calls and responses, some calls and responses in C# and likely some more.
(Note the previous sentence is vague on purpose. I want that you create a complete illustration.)

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

![ScreenshotOfTerminal](images/ChirpStartPage.png)

## How to run test suite locally


Our test suite is completed of 20 tests, of which are either Unit-, Integration- or End2End-tests. 3 of the 20 tests are made with Playwright. 

The **unit tests** mainly test that our SQL database and Chirp.Core work as expected and create the corresponding object DTO's. 

The **integration tests** check the website displays the correct information, and that the client is on the correct page (Public timeline / Private timeline), with the relevant info for this page. 

The **end2end tests**, tests the program's overall functionality from start to end, including login and authorization. 


***Software needed***: 
- VSCODE
- .NET 7.0
- Playwright

**1. Running the tests**
1. Confirm you have Playwright installed and open. 
2. In VSCODE, start a new terminal from the /Chirp folder 
3. To run the first part of the test suite (except from Playwright tests), type in the terminal: 
		
		dotnet test

4. **Expected result *1st Part* ** should look alike, with 17 passed tests.: 

	![ScreenshotOfTerminal](images/FirstPartOfTests.png)

5. Now, open a second terminal, from the /test/PlaywrightTests folder
6. To run the second part of the test suite (the Playwright tests), type in this terminal: 

		dotnet test

4. **Expected result *2nd Part*** should look alike, with 3 passed tests.: (OBS: You may have to manually press green button 'Authorize' at one point during the testing).

	![ScreenshotOfTerminal](images/3PassedPlaywrightTests.png)

# Ethics

## License
We use an MIT License for software. We chose this license as it is very simple and permissive. It grants permission to modifying, distributing, private and commercial use. As there is no warranty and liability, the copyright holders are not responsible for how the software is used. 

## LLMs, ChatGPT, CoPilot, and others
We did not make use of any LLMs in our project. 
