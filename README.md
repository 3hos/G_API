# G_API
This project is a simple ASP.NET Core Web API.

This API integrates various music-related services to provide a comprehensive platform for searching and recommending song tabs, along with additional song information such as complexity, genre, and chord fingerings.

## Implemented API functionality:

### Songster API:
- Search for song tabs by title and author, generate links to them.
### Songster.com
- Parsing the website to retrieve tab complexity information.

### Spotify API:
- Search for additional song information necessary for generating recommendations.
- Formulate a list of recommended songs.

### Theaudiodb API:
- Search for the song's genre.

### Uberchord API:
- Search for chord fingerings.
- Find chord fingerings with the same root note.

### DynamoDB
- This project uses NOSQL database.

## Client capabilities:

- Ability to find a list of songs with similar titles or authors to the search query.
- The song list includes title, author, genre, tab complexity, links to tab websites, and search queries for guitar tutorials related to that track.
- After searching, the user can add one of the songs to their favorites list.
- The user can view their list of favorite tracks and remove specific entries by providing their respective numbers in the list.
- The user can generate a list of recommended songs based on their favorite tracks.
- By entering a chord name, the user can obtain its tab and compatible keys or a list of similar chords with relevant information.

## Telegram bot using this API

![image](https://github.com/3hos/G_API/assets/106525329/1b8162f5-f317-4bf7-95ba-6755dda8c500)
![image](https://github.com/3hos/G_API/assets/106525329/b76f48d9-de14-45e6-b5f6-5fdb889a26b3)
![image](https://github.com/3hos/G_API/assets/106525329/47c88d8b-72fd-4486-af02-9c9f914fe1bc)
![image](https://github.com/3hos/G_API/assets/106525329/83ce16a7-28de-407e-a24f-a03ac4701c51)
![image](https://github.com/3hos/G_API/assets/106525329/14583e5c-ec7d-4621-a8c0-658088deefa0)




