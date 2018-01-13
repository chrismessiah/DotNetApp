# DotNetApp

Dockerized .NET Core 2.0.x Razor app boilerplate with Postgres, Gulp, Sass, ES6, custom .env vars and more

## Imperfections

Nothing is perfect and this project is **very far** from it. Bellow I document oddities to avoid unnecessary headices.

### 0. Not by any means secure(!)
Throughout the project there are several notes regarding thing that should be changes/updated/added in order to achive a better overall security. A few examples are

* CORS is inactive
* Enviroment variables may be accessed through multiple container layers and/or process
* No viable method for error handling in controllers when assuming existing request parameters/body

### 1. Must-have env vars (production only)
Make sure you set these env vars using `docker ... -e KEY="VALUE"` or similar

* `DOTNET_ENV="Production"`
* `CONNECTION_STRING="User ID=username;Password=password;Server=localhost;Port=5432;Database=dbname"`

### 2. `appsettings.json` is overrided

Many enviroment variables are stored in the `env` dict in `Globals.cs` to allow passing enviroment through docker or reading from `.env`, this causes `appsettings.json` to become unreliable. Currently both are being used and should be migrated to a better solution.

### 3. Migrations run with `dotnet run` (production only)
To allow migrations to be executed automatically they are being run on app start on production enviroments. This could be dangerous since **data may be lost** unexpectedly if an unwanted migration is commited and pushed

### 4. Connecting to Host Postgres from inside Docker container using localhost wont work.
This is due to two main reasons.

1. Postgres does by defualt not allow remote connections
2. Due to Docker networking localhost inside container is not localhost outside it.

## Notes

* The .NET app will always listen to **port 5000**
* To rename the just replace all DotNetApp entries with the name of the project as well as the project root folder
