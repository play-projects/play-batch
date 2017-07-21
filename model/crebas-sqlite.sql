drop table CATEGORY;

drop table CHARACTER;

drop table COLLECTION;

drop table GENRE;

drop table LANGUAGE;

drop table MOVIE;

drop table MOVIE_GENRE;

drop table PERSON;

drop table QUALITY;

drop table SOURCE;

drop table TORRENT;

create table CATEGORY (
CATEGORY_ID          INTEGER PRIMARY KEY            AUTOINCREMENT,
CATEGORY_NAME        VARCHAR(50)                    not null,
CATEGORY_CREATED_AT  DATE                           not null
);

create table PERSON (
PERSON_ID            INTEGER PRIMARY KEY            AUTOINCREMENT,
PERSON_FIRSTNAME     VARCHAR(250)                   not null,
PERSON_LASTNAME      VARCHAR(250)                   not null,
PERSON_PROFILE_PATH  VARCHAR(1000),
PERSON_CREATED_AT    DATE                           not null
);

create table COLLECTION (
COLLECTION_ID        INTEGER PRIMARY KEY            AUTOINCREMENT,
COLLECTION_TMDB_ID   INTEGER                        not null,
COLLECTION_NAME      VARCHAR(1000)                  not null,
COLLECTION_POSTER_PATH VARCHAR(1000),
COLLECTION_BACKDROP_PATH VARCHAR(1000),
COLLECTION_CREATED_AT DATE                           not null
);

create table MOVIE (
MOVIE_ID             INTEGER PRIMARY KEY            AUTOINCREMENT,
COLLECTION_ID        INTEGER,
MOVIE_TRAKT_ID       INTEGER                        not null,
MOVIE_IMDB_ID        VARCHAR(25)                    not null,
MOVIE_TMDB_ID        INTEGER                        not null,
MOVIE_TITLE          VARCHAR(500)                   not null,
MOVIE_TITLE_SEARCH   VARCHAR(500)					not null,
MOVIE_ORIGINAL_TITLE VARCHAR(500),
MOVIE_YEAR           INTEGER                        not null,
MOVIE_ORIGINAL_LANGUAGE VARCHAR(50),
MOVIE_OVERVIEW       LONG VARCHAR                   not null,
MOVIE_TAGLINE        VARCHAR(1000),
MOVIE_POPULARITY     DECIMAL                        not null,
MOVIE_RELEASE_DATE   DATE,
MOVIE_VOTE_AVERAGE   DECIMAL                        not null,
MOVIE_VOTE_COUNT     INTEGER                        not null,
MOVIE_RUNTIME        INTEGER,
MOVIE_BACKDROP_PATH  VARCHAR(1000),
MOVIE_POSTER_PATH    VARCHAR(1000),
MOVIE_CREATED_AT     DATE                           not null,
foreign key (COLLECTION_ID)
      references COLLECTION (COLLECTION_ID)
);

create table CHARACTER (
PERSON_ID            INTEGER PRIMARY KEY            AUTOINCREMENT,
MOVIE_ID             INTEGER                        not null,
CHARACTER_NAME       VARCHAR(250)                   not null,
CHARACTER_ORDER      INTEGER                        not null,
CHARACTER_CREATED_AT DATE                           not null,
foreign key (PERSON_ID)
      references PERSON (PERSON_ID),
foreign key (MOVIE_ID)
      references MOVIE (MOVIE_ID)
);

create table GENRE (
GENRE_ID             INTEGER PRIMARY KEY            AUTOINCREMENT,
GENRE_TMDB_ID        INTEGER                        not null,
GENRE_NAME           VARCHAR(250)                   not null,
GENRE_CREATED_AT     DATE                           not null
);

create table LANGUAGE (
LANGUAGE_ID          INTEGER PRIMARY KEY            AUTOINCREMENT,
LANGUAGE_NAME        VARCHAR(50)                    not null,
LANGUAGE_CREATED_AT  DATE                           not null
);

create table MOVIE_GENRE (
GENRE_ID             INTEGER                        not null,
MOVIE_ID             INTEGER                        not null,
primary key (GENRE_ID, MOVIE_ID),
foreign key (MOVIE_ID)
      references MOVIE (MOVIE_ID),
foreign key (GENRE_ID)
      references GENRE (GENRE_ID)
);

create table QUALITY (
QUALITY_ID           INTEGER PRIMARY KEY            AUTOINCREMENT,
QUALITY_NAME         VARCHAR(50)                    not null,
QUALITY_CREATED_AT   DATE                           not null
);

create table SOURCE (
SOURCE_ID			 INTEGER PRIMARY KEY			AUTOINCREMENT,
SOURCE_NAME			 VARCHAR(250)					not null,
SOURCE_CREATED_AT	 DATE							not null
);

create table TORRENT (
TORRENT_ID           INTEGER PRIMARY KEY            AUTOINCREMENT,
SOURCE_ID			 INTEGER						not null,
MOVIE_ID             INTEGER,
CATEGORY_ID          INTEGER                        not null,
LANGUAGE_ID          INTEGER                        not null,
QUALITY_ID           INTEGER                        not null,
TORRENT_GUID         VARCHAR(250)					not null,
TORRENT_NAME         VARCHAR(250)                   not null,
TORRENT_SLUG         VARCHAR(250),
TORRENT_YEAR         INTEGER,
TORRENT_SIZE         BIGINT                         not null,
TORRENT_SEEDERS      INTEGER,
TORRENT_LEECHERS     INTEGER,
TORRENT_COMPLETED    INTEGER,
TORRENT_LINK		 VARCHAR(1000)					not null,
TORRENT_CREATED_AT   DATE                           not null,
foreign key(SOURCE_ID)
	  references SOURCE (SOURCE_ID),
foreign key (MOVIE_ID)
      references MOVIE (MOVIE_ID),
foreign key (CATEGORY_ID)
      references CATEGORY (CATEGORY_ID),
foreign key (LANGUAGE_ID)
      references LANGUAGE (LANGUAGE_ID),
foreign key (QUALITY_ID)
      references QUALITY (QUALITY_ID)
);

