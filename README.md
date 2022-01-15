# Your Game List

I always wanted to make a list of all the computer games I had the chance to play. I know that there are many sites that provides this functionality, but I came up with an idea to write one like those on my own. Or at least the back-end, because front has never really been my thing... as a nice project to develop my C# skills. 

This project is divided into 4 smaller ones.
- IGDB.Model
- IGDB.Scraper
- YGL.API
- YGL.Model


#### [IGDB.Model](https://github.com/Gazda99/Your-Games-List/tree/main/IGDB.Model)
Contains a wrapper for IGDB Api response objects and custom Json Converter for their responses. This project uses an [IGDB C# Wrapper](https://github.com/kamranayub/igdb-dotnet) (with some changes) without which it would be very difficult for me to work with the IGDB responses.

#### [IGDB.Scraper](https://github.com/Gazda99/Your-Games-List/tree/main/IGDB.Scraper)
This projects aims to automate the process of copying  the data from IGDB database to YGL database.

#### [YGL.Model](https://github.com/Gazda99/Your-Games-List/tree/main/YGL.Model)
Provides the data model using Entity Framework for YGL Api for easy connection to YGL database.

#### [YGL.API](https://github.com/Gazda99/Your-Games-List/tree/main/YGL.API)
Main heart of this project. Contains all logic for requests/responses, logging in, connecting to DB, caching etc.

---
It runs on .Net 6.

---
