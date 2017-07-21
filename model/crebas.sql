/*==============================================================*/
/* Nom de SGBD :  Microsoft SQL Server 2008                     */
/* Date de création :  18/07/2017 01:33:27                      */
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
           where  id = object_id('SOURCE')
            and   type = 'U')
   drop table SOURCE
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
   CATEGORY_ID          int					 identity,
   CATEGORY_NAME        varchar(50)          not null,
   CATEGORY_CREATED_AT  datetime             not null,
   constraint PK_CATEGORY primary key nonclustered (CATEGORY_ID)
)
go

/*==============================================================*/
/* Table : CHARACTER                                            */
/*==============================================================*/
create table CHARACTER (
   PERSON_ID            int                  not null,
   MOVIE_ID             int                  not null,
   CHARACTER_NAME       varchar(250)         not null,
   CHARACTER_ORDER      int                  not null,
   CHARACTER_CREATED_AT datetime             not null,
   constraint PK_CHARACTER primary key nonclustered (PERSON_ID, MOVIE_ID)
)
go

/*==============================================================*/
/* Table : COLLECTION                                           */
/*==============================================================*/
create table COLLECTION (
   COLLECTION_ID        int					 identity,
   COLLECTION_TMDB_ID   int                  not null,
   COLLECTION_NAME      varchar(1000)        not null,
   COLLECTION_POSTER_PATH varchar(1000)      null,
   COLLECTION_BACKDROP_PATH varchar(1000)    null,
   COLLECTION_CREATED_AT datetime            not null,
   constraint PK_COLLECTION primary key nonclustered (COLLECTION_ID)
)
go

/*==============================================================*/
/* Table : GENRE                                                */
/*==============================================================*/
create table GENRE (
   GENRE_ID             int					 identity,
   GENRE_TMDB_ID        int                  not null,
   GENRE_NAME           varchar(250)         not null,
   GENRE_CREATED_AT     datetime             not null,
   constraint PK_GENRE primary key nonclustered (GENRE_ID)
)
go

/*==============================================================*/
/* Table : LANGUAGE                                             */
/*==============================================================*/
create table LANGUAGE (
   LANGUAGE_ID          int					 identity,
   LANGUAGE_NAME        varchar(50)          not null,
   LANGUAGE_CREATED_AT  datetime             not null,
   constraint PK_LANGUAGE primary key nonclustered (LANGUAGE_ID)
)
go

/*==============================================================*/
/* Table : MOVIE                                                */
/*==============================================================*/
create table MOVIE (
   MOVIE_ID             int					 identity,
   COLLECTION_ID        int                  null,
   MOVIE_TRAKT_ID       int                  not null,
   MOVIE_IMDB_ID        varchar(25)          not null,
   MOVIE_TMDB_ID        int                  not null,
   MOVIE_TITLE          varchar(500)         not null,
   MOVIE_TITLE_SEARCH   varchar(500)         null,
   MOVIE_ORIGINAL_TITLE varchar(500)         null,
   MOVIE_YEAR           int                  not null,
   MOVIE_ORIGINAL_LANGUAGE varchar(50)       null,
   MOVIE_OVERVIEW       varchar(2500)        not null,
   MOVIE_TAGLINE        varchar(1000)        null,
   MOVIE_POPULARITY     decimal              not null,
   MOVIE_RELEASE_DATE   datetime             null,
   MOVIE_VOTE_AVERAGE   decimal              not null,
   MOVIE_VOTE_COUNT     int                  not null,
   MOVIE_RUNTIME        int                  null,
   MOVIE_BACKDROP_PATH  varchar(1000)        null,
   MOVIE_POSTER_PATH    varchar(1000)        null,
   MOVIE_CREATED_AT     datetime             not null,
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
   PERSON_ID            int					 identity,
   PERSON_FIRSTNAME     varchar(250)         not null,
   PERSON_LASTNAME      varchar(250)         not null,
   PERSON_PROFILE_PATH  varchar(1000)        null,
   PERSON_CREATED_AT    datetime             not null,
   constraint PK_PERSON primary key nonclustered (PERSON_ID)
)
go

/*==============================================================*/
/* Table : QUALITY                                              */
/*==============================================================*/
create table QUALITY (
   QUALITY_ID           int					 identity,
   QUALITY_NAME         varchar(50)          not null,
   QUALITY_CREATED_AT   datetime             not null,
   constraint PK_QUALITY primary key nonclustered (QUALITY_ID)
)
go

/*==============================================================*/
/* Table : SOURCE                                               */
/*==============================================================*/
create table SOURCE (
   SOURCE_ID            int              identity,
   SOURCE_NAME          varchar(250)          null,
   SOURCE_CREATED_AT    datetime             null,
   constraint PK_SOURCE primary key nonclustered (SOURCE_ID)
)
go

/*==============================================================*/
/* Table : TORRENT                                              */
/*==============================================================*/
create table TORRENT (
   TORRENT_ID           int                  identity,
   SOURCE_ID            int                  not null,
   MOVIE_ID             int                  null,
   CATEGORY_ID          int                  not null,
   LANGUAGE_ID          int                  not null,
   QUALITY_ID           int                  not null,
   TORRENT_GUID         varchar(250)         not null,
   TORRENT_NAME         varchar(250)         not null,
   TORRENT_SLUG         varchar(250)         null,
   TORRENT_YEAR         int                  null,
   TORRENT_SIZE         bigint               not null,
   TORRENT_SEEDERS      int                  null,
   TORRENT_LEECHERS     int                  null,
   TORRENT_COMPLETED    int                  null,
   TORRENT_CREATED_AT   datetime             not null,
   TORRENT_LINK			varchar(1000)        not null,
   constraint PK_TORRENT primary key nonclustered (TORRENT_ID)
)
go

