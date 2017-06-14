drop table CATEGORY;

drop table CHARACTER;

drop table COLLECTION;

drop table GENRE;

drop table LANGUAGE;

drop table MOVIE;

drop table MOVIE_GENRE;

drop table PERSON;

drop table QUALITY;

drop table TORRENT;

create table CATEGORY (
CATEGORY_ID          NUMERIC(6)                     not null,
CATEGORY_NAME        VARCHAR(50)                    not null,
CATEGORY_CREATED_AT  DATE                           not null,
primary key (CATEGORY_ID)
);

create table PERSON (
PERSON_ID            NUMERIC(6)                     not null,
PERSON_FIRSTNAME     VARCHAR(250)                   not null,
PERSON_LASTNAME      VARCHAR(250)                   not null,
PERSON_PROFILE_PATH  VARCHAR(1000),
PERSON_CREATED_AT    DATE                           not null,
primary key (PERSON_ID)
);

create table COLLECTION (
COLLECTION_ID        NUMERIC(6)                     not null,
COLLECTION_TMDB_ID   INTEGER                        not null,
COLLECTION_NAME      VARCHAR(1000)                  not null,
COLLECTION_POSTER_PATH VARCHAR(1000),
COLLECTION_BACKDROP_PATH VARCHAR(1000),
COLLECTION_CREATED_AT DATE                           not null,
primary key (COLLECTION_ID)
);

create table MOVIE (
MOVIE_ID             NUMERIC(6)                     not null,
COLLECTION_ID        INTEGER,
MOVIE_TRAKT_ID       INTEGER                        not null,
MOVIE_IMDB_ID        VARCHAR(25)                    not null,
MOVIE_TMDB_ID        INTEGER                        not null,
MOVIE_TITLE          VARCHAR(500)                   not null,
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
primary key (MOVIE_ID),
foreign key (COLLECTION_ID)
      references COLLECTION (COLLECTION_ID)
);

create table CHARACTER (
PERSON_ID            INTEGER                        not null,
MOVIE_ID             INTEGER                        not null,
CHARACTER_NAME       VARCHAR(250)                   not null,
CHARACTER_ORDER      INTEGER                        not null,
CHARACTER_CREATED_AT DATE                           not null,
primary key (PERSON_ID, MOVIE_ID),
foreign key (PERSON_ID)
      references PERSON (PERSON_ID),
foreign key (MOVIE_ID)
      references MOVIE (MOVIE_ID)
);

create table GENRE (
GENRE_ID             NUMERIC(6)                     not null,
GENRE_TMDB_ID        INTEGER                        not null,
GENRE_NAME           VARCHAR(250)                   not null,
GENRE_CREATED_AT     DATE                           not null,
primary key (GENRE_ID)
);

create table LANGUAGE (
LANGUAGE_ID          NUMERIC(6)                     not null,
LANGUAGE_NAME        VARCHAR(50)                    not null,
LANGUAGE_CREATED_AT  DATE                           not null,
primary key (LANGUAGE_ID)
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
QUALITY_ID           NUMERIC(6)                     not null,
QUALITY_NAME         VARCHAR(50)                    not null,
QUALITY_CREATED_AT   DATE                           not null,
primary key (QUALITY_ID)
);

create table TORRENT (
TORRENT_ID           NUMERIC(6)                     not null,
MOVIE_ID             INTEGER,
CATEGORY_ID          INTEGER                        not null,
LANGUAGE_ID          INTEGER                        not null,
QUALITY_ID           INTEGER                        not null,
TORRENT_T411_ID      INTEGER                        not null,
TORRENT_NAME         VARCHAR(250)                   not null,
TORRENT_SLUG         VARCHAR(250),
TORRENT_YEAR         INTEGER,
TORRENT_SIZE         BIGINT                         not null,
TORRENT_SEEDERS      INTEGER,
TORRENT_LEECHERS     INTEGER,
TORRENT_COMPLETED    INTEGER,
TORRENT_CREATED_AT   DATE                           not null,
primary key (TORRENT_ID),
foreign key (MOVIE_ID)
      references MOVIE (MOVIE_ID),
foreign key (CATEGORY_ID)
      references CATEGORY (CATEGORY_ID),
foreign key (LANGUAGE_ID)
      references LANGUAGE (LANGUAGE_ID),
foreign key (QUALITY_ID)
      references QUALITY (QUALITY_ID)
);

