## API to retrieve countries and it's data by country's cca3 code (ex: LKA, IND)

# Endpoints
*ports may change when running the app*

Get one country by ID (IND is the cca3 code for india)
https://localhost:7080/api/country/IND

Get multiple countries by ids
https://localhost:7080/api/country/GetByCodes?ids=LKA&ids=IND&ids=GBR

# Configure DB
- open SSMS and run following commands to create a new database and table
  
CREATE DATABASE CountryDB;

USE CountryDB;

CREATE TABLE Countries (
    Id VARCHAR(3) PRIMARY KEY,
    Name NVARCHAR(100),
    Region NVARCHAR(100),
    Population BIGINT
);


# Set Connection String
- you can set the connection string following ways
  * Enviorment variable (Default)
    set connection string in eviorment varible by running following command in windows powershell. (use your own connection string)
    setx COUNTRY_DB_CONN "Your_Connection_String"
    
  * if the enviorment variable is not set, it will be retrieved from appsettings.Development.Json (as a fallback)
    - if failed to set enviorment variable, go to appsettings.Development.Json and set *your connection string* in "ConnectionStrings"."DefaultConnection"

Run the app using visual studio :) 

# installed packages:
API calls:
 - Microsoft.Extensions.Http
Logging:
 - Serilog.AspNetCore
 - Serilog.Sinks.File



