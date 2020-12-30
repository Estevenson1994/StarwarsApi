![Star Wars Api](https://github.com/Estevenson1994/StarwarsApi/workflows/.NET/badge.svg)

```
   .           .        .                     .        .            .
             .               .    .          .              .   .         .
               _________________      ____         __________
 .       .    /                 |    /    \    .  |          \
     .       /    ______   _____| . /      \      |    ___    |     .     .
             \    \    |   |       /   /\   \     |   |___>   |
           .  \    \   |   |      /   /__\   \  . |         _/               .
 .     ________>    |  |   | .   /            \   |   |\    \_______    .
      |            /   |   |    /    ______    \  |   | \           |
      |___________/    |___|   /____/      \____\ |___|  \__________|    .
  .     ____    __  . _____   ____      .  __________   .  _________
       \    \  /  \  /    /  /    \       |          \    /         |      .
        \    \/    \/    /  /      \      |    ___    |  /    ______|  .
         \              /  /   /\   \ .   |   |___>   |  \    \
   .      \            /  /   /__\   \    |         _/.   \    \            +
           \    /\    /  /            \   |   |\    \______>    |   .
            \  /  \  /  /    ______    \  |   | \              /          .
 .       .   \/    \/  /____/      \____\ |___|  \____________/  
                               .                                        .
     .                           .         .               .                 .
                .                                   .            .
```

[Tech](#technologies-used) | [How to use](#how-to-use) | [Criteria and usage](#criteria-and-usage) | [Challenges](#challenges)

# Star Wars API

This is a Star Wars API created to allow users to retrieve information about some of the Star Wars films. Data for this api has been retrieved from 
[swapi.dev](https://swapi.dev/)

## Technologies used

- C# with the .NET framework for development (version .NET 5.0)
- Entity Framework Core for database quieries (I used EFcore in memory database as it was the fasted database to set up)
- xUnit for testing
- GitHub actions for CI

## How to use

To run this program locally:
- Ensure you have [.NET 5](https://dotnet.microsoft.com/download) downloaded and installed
- Clone and download this repository `git@github.com:Estevenson1994/StarwarsApi.git`
- Go into the solution `cd StarwarsApi`
- Run `dotnet build --project StarWarsApi` to build the project or `dotnet run --project StarWarsApi` to build and run

To run the tests:
- Run `dotnet test StarWarsTests`

## Criteria and usage

This program was built around the following criteria:

1. Allow clients to retreive a list of all films. This should include the film name and the name of all the characters featured.
- The end point to retrieve this is:

`GET: https://localhost:5001/Starwars/Films`

This will return a list of all films, names of all characters, species and planets featured in that film (species and planets were included for criteria 4. and 5.).

2. Allow clients to create a character. The character should have a name, an optional birth year, once species and one or more films.
- The end point to perform this is:

`POST: https://localhost:5001/Starwars/Characters`
This requireds a body containing a character object (if using postman, set body to 'raw' and format to be 'json'. The json object should be in the following format:

```
{
  "name": "name of character to add",
  "films": [
    "first film",
    "second film"
    ],
  "species": "name of species"
}
  
```
This will return a HttpStatus status code 201 created response, if the character was successfully added, or a status code 422 unprocessable entity response if any of the submitted data was incorrect.

Assumptions made: 
  - You cannot add a character that has the same name as one already stored in the database.
  - You can only specify a film that is currently stored in the database.
  - Each character can have at most one species.
  - The species must already exist in the database.
  
 You can check the character has been added by using the following endpoint:
 
 `GET: https://localhost:5001/Starwars/Characters`
 
 This will return a list of all characters, with information about the films they are in and what species they are. The newly added character will appear at the end of the list.
  
  3. Update endpoint created in 1 to include paging.
  - The route to retrieve this is:
  
  `GET: https://localhost:5001/Starwars/Films/pageNumber/{pageNumber}/pageSize/{pageSize}` where '{pageNumber}' should be replaced with the page you want to get information from and '{pageSize}' should be replaced with the total number of results required for each page.
  
  This will return the same type of data as in part 1.
  
  4. Update endpoint created in 1 to allow filtering by species. The client should be able to specify a species and only films that featured that species should be returned.
  
  - To achieve this, you can use the route specified in part 1 or part 3, and add on the query `?species={species-you-want-to-filter-by}`. For example:
  
  `GET: https://localhost:5001/Starwars/Films?species=Human`
  
  This will return a list of only the films that include humans.
  
  5. Update endpoint created in 1 to allow filtering by planets. The client should be able to specify a planet and only films that featured that planet should be returned.
  
  - To achieve this, you can use the route specified in part 1 or part 3, and add on the query `?planet={planet-you-want-to-filter-by}`. For example:
  
   `GET: https://localhost:5001/Starwars/Films?planet=Tatooine`
   
   This will return a list of only the films that include the planet Tatooine.
   
  `GET: https://localhost:5001/Starwars/Films?planet=Tatooine&species=Human`
  
  This will return a list of only the films that include humans and the planet Tatooine
  
  ## Challenges
  
  The main challenge I faced during this tech test was writing the tests. Tests are extremely important, however I have little experience writing them due to the nature of the project I have been working on. I took this as an opportunity to learn how to write tests with xUnit, however it is an area I am keen to develop in my next role.
  
  This was also the first API application I have started from scratch, so this was a nice opportunity to teach myself how to do that.







