/*==============================================================*/
/* Nom de SGBD :  Microsoft SQL Server 2008                     */
/* Date de création :  11/06/2017 01:24:14                      */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('CATEGORY')
            and   type = 'U')
   drop table CATEGORY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CHARACTER')
            and   type = 'U')
   drop table CHARACTER
go

if exists (select 1
            from  sysobjects
           where  id = object_id('COLLECTION')
            and   type = 'U')
   drop table COLLECTION
go

if exists (select 1
            from  sysobjects
           where  id = object_id('GENRE')
            and   type = 'U')
   drop table GENRE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('LANGUAGE')
            and   type = 'U')
   drop table LANGUAGE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('MOVIE')
            and   type = 'U')
   drop table MOVIE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('MOVIE_GENRE')
            and   type = 'U')
   drop table MOVIE_GENRE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('PERSON')
            and   type = 'U')
   drop table PERSON
go

if exists (select 1
            from  sysobjects
           where  id = object_id('QUALITY')
            and   type = 'U')
   drop table QUALITY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TORRENT')
            and   type = 'U')
   drop table TORRENT
go

/*==============================================================*/
/* Table : CATEGORY                                             */
/*==============================================================*/
create table CATEGORY (
   CATEGORY_ID          integer              identity,
   CATEGORY_NAME        varchar(25)          null,
   constraint PK_CATEGORY primary key nonclustered (CATEGORY_ID)
)
go

/*==============================================================*/
/* Table : CHARACTER                                            */
/*==============================================================*/
create table CHARACTER (
   PERSON_ID            int                  not null,
   MOVIE_ID             int                  not null,
   CHARACTER_NAME       varchar(250)         null,
   CHARACTER_ORDER      int                  null,
   constraint PK_CHARACTER primary key nonclustered (PERSON_ID, MOVIE_ID)
)
go

/*==============================================================*/
/* Table : COLLECTION                                           */
/*==============================================================*/
create table COLLECTION (
   COLLECTION_ID        integer              identity,
   COLLECTION_TMDB_ID   int                  null,
   COLLECTION_NAME      varchar(100)         null,
   COLLECTION_POSTER_PATH varchar(100)         null,
   COLLECTION_BACKDROP_PATH varchar(100)         null,
   constraint PK_COLLECTION primary key nonclustered (COLLECTION_ID)
)
go

/*==============================================================*/
/* Table : GENRE                                                */
/*==============================================================*/
create table GENRE (
   GENRE_ID             integer              identity,
   GENRE_TMDB_ID        int                  null,
   GENRE_NAME           varchar(100)         null,
   constraint PK_GENRE primary key nonclustered (GENRE_ID)
)
go

/*==============================================================*/
/* Table : LANGUAGE                                             */
/*==============================================================*/
create table LANGUAGE (
   LANGUAGE_ID          integer              identity,
   LANGUAGE_NAME        varchar(25)          null,
   constraint PK_LANGUAGE primary key nonclustered (LANGUAGE_ID)
)
go

/*==============================================================*/
/* Table : MOVIE                                                */
/*==============================================================*/
create table MOVIE (
   MOVIE_ID             integer              identity,
   COLLECTION_ID        int                  null,
   MOVIE_TRAKT_ID       int                  null,
   MOVIE_IMDB_ID        varchar(10)          null,
   MOVIE_TMDB_ID        int                  null,
   MOVIE_TITLE          varchar(250)         null,
   MOVIE_ORIGINAL_TITLE varchar(250)         null,
   MOVIE_YEAR           int                  null,
   MOVIE_ORIGINAL_LANGUAGE varchar(25)          null,
   MOVIE_OVERVIEW       varchar(2500)        null,
   MOVIE_TAGLINE        varchar(250)         null,
   MOVIE_POPULARITY     decimal              null,
   MOVIE_RELEASE_DATE   datetime             null,
   MOVIE_VOTE_AVERAGE   decimal              null,
   MOVIE_VOTE_COUNT     int                  null,
   MOVIE_RUNTIME        int                  null,
   MOVIE_BACKDROP_PATH  varchar(100)         null,
   MOVIE_POSTER_PATH    varchar(100)         null,
   constraint PK_MOVIE primary key nonclustered (MOVIE_ID)
)
go

/*==============================================================*/
/* Table : MOVIE_GENRE                                          */
/*==============================================================*/
create table MOVIE_GENRE (
   GENRE_ID             int                  not null,
   MOVIE_ID             int                  not null,
   constraint PK_MOVIE_GENRE primary key nonclustered (GENRE_ID, MOVIE_ID)
)
go

/*==============================================================*/
/* Table : PERSON                                               */
/*==============================================================*/
create table PERSON (
   PERSON_ID            integer              identity,
   PERSON_FIRSTNAME     varchar(250)         null,
   PERSON_LASTNAME      varchar(250)         null,
   PERSON_PROFILE_PATH  varchar(100)         null,
   constraint PK_PERSON primary key nonclustered (PERSON_ID)
)
go

/*==============================================================*/
/* Table : QUALITY                                              */
/*==============================================================*/
create table QUALITY (
   QUALITY_ID           integer              identity,
   QUALITY_NAME         varchar(25)          null,
   constraint PK_QUALITY primary key nonclustered (QUALITY_ID)
)
go

/*==============================================================*/
/* Table : TORRENT                                              */
/*==============================================================*/
create table TORRENT (
   TORRENT_ID           integer              identity,
   MOVIE_ID             int                  null,
   CATEGORY_ID          int                  not null,
   LANGUAGE_ID          int                  not null,
   QUALITY_ID           int                  not null,
   TORRENT_T411_ID      int                  null,
   TORRENT_NAME         varchar(250)         null,
   TORRENT_SLUG         varchar(250)         null,
   TORRENT_YEAR         int                  null,
   TORRENT_SIZE         bigint               null,
   TORRENT_SEEDERS      int                  null,
   TORRENT_LEECHERS     int                  null,
   TORRENT_COMPLETED    int                  null,
   constraint PK_TORRENT primary key nonclustered (TORRENT_ID)
)
go

