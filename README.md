# fractalsolutions-api
True Layer app integration

Repository contains an aspnet core service

pre-requisites:

asp.net core 3.1 SDK
visual studio
Sql server and management studio

How to run api:

- Load the FractalSolutions.Api.sln in Visual studio
- add client id and secret for your truelayer client
- on true layer setup redirect url to point to the api
- run api
- use auth link builder on true layer which now now redirect to the api and get access code
- api has swagger so no need for post man, just add the bearer token on there once obtained 
- you should be able to get transactions for loggin user
