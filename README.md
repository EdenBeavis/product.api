# Xero Coding Challenge
## Design Decisions
### Why .net core/.net 5.0.?
.net core is a cross platform tool that will work on Mac, Windows and Linux, so there should be no worry on whether this will work on the testers device.
It is good for data processing and so it seemed like a perfect fit. 

### CQRS And Entity Framework
Cqrs helps with the [single-responsibility principle](https://en.wikipedia.org/wiki/Single-responsibility_principle). Requests will usually have a focused task so there won't be a muddle of code for different purposes.
Downsides? It is easy for commands and queries to do the same thing when the project is scaled up (different developers may not know the request is already created).
Entity framework is great way to implement the repository pattern, allowing for a sergerate interface that is easy to extend but hard to modify. It also has the nice ability to interface with other db's easily. For the test I use an in memory database as an example.

### Why not update the database to sql server or mysql
Honestly, mostly time.
Ideally I would move away from using sqllite but mostly due to time constraints, I couldn't change the datasource.

## Running the project
- Download or pull this repo
### If you have Visual Studio installed
1. Open the soloution
2. Hit the start button and a browser should open, navigating to the swagger url. If it doesn't it will be located here: https://localhost:44335/index.html.
3. Goto excute heading below.
### If you DON'T have Visual studio installed
1. Open up a command console or terminal window and naviate to the root of the project.
2. Run the "dotnet restore" command.
3. Navigate to "prodcut.api" project folder in terminal/cmd.
4. Run the "dotnet run" command which will launch the api.
5. In a browser navigate to the swagger page: https://localhost:5001/index.html.
6. Goto excute heading below.
### Execute
1. In the "Product API" swagger page, click an endpoint to "try it out".
2. Request bodies should be auto filled in but you can modify them as needed.
3. Press the "Execute" button once you have input details.
4. The output will be displayed in the "Responses" section.
5. You are done!

## Running the tests
### If you have Visual Studio installed
1. In the test explorer, hit the run all tests button.
2. The 6 tests should pass.
### If you DON'T have Visual studio installed
1. Open up a command console or terminal window and naviate to the root of the project.
2. Run the "dotnet restore" command.
3. Run the "dotnet test" command.
4. The 51 tests should pass -> Displayed in green text.