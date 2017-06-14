/*==============================================================*/
/* Nom de SGBD :  MySQL 5.0                                     */
/* Date de création :  14/06/2017 18:31:48                      */
/*==============================================================*/


drop table if exists CATEGORY;

drop table if exists CHARACTER;

drop table if exists COLLECTION;

drop table if exists GENRE;

drop table if exists LANGUAGE;

drop table if exists MOVIE;

drop table if exists MOVIE_GENRE;

drop table if exists PERSON;

drop table if exists QUALITY;

drop table if exists TORRENT;

/*==============================================================*/
/* Table : CATEGORY                                             */
/*==============================================================*/
create table CATEGORY
(
   CATEGORY_ID          int not null auto_increment,
   CATEGORY_NAME        varchar(50) not null,
   CATEGORY_CREATED_AT  date not null,
   primary key (CATEGORY_ID)
);

/*==============================================================*/
/* Table : CHARACTER                                            */
/*==============================================================*/
create table CHARACTER
(
   PERSON_ID            int not null,
   MOVIE_ID             int not null,
   CHARACTER_NAME       varchar(250) not null,
   CHARACTER_ORDER      int not null,
   CHARACTER_CREATED_AT date not null,
   primary key (PERSON_ID, MOVIE_ID)
);

/*==============================================================*/
/* Table : COLLECTION                                           */
/*==============================================================*/
create table COLLECTION
(
   COLLECTION_ID        int not null auto_increment,
   COLLECTION_TMDB_ID   int not null,
   COLLECTION_NAME      varchar(1000) not null,
   COLLECTION_POSTER_PATH varchar(1000),
   COLLECTION_BACKDROP_PATH varchar(1000),
   COLLECTION_CREATED_AT date not null,
   primary key (COLLECTION_ID)
);

/*==============================================================*/
/* Table : GENRE                                                */
/*==============================================================*/
create table GENRE
(
   GENRE_ID             int not null auto_increment,
   GENRE_TMDB_ID        int not null,
   GENRE_NAME           varchar(250) not null,
   GENRE_CREATED_AT     date not null,
   primary key (GENRE_ID)
);

/*==============================================================*/
/* Table : LANGUAGE                                             */
/*==============================================================*/
create table LANGUAGE
(
   LANGUAGE_ID          int not null auto_increment,
   LANGUAGE_NAME        varchar(50) not null,
   LANGUAGE_CREATED_AT  date not null,
   primary key (LANGUAGE_ID)
);

/*==============================================================*/
/* Table : MOVIE                                                */
/*==============================================================*/
create table MOVIE
(
   MOVIE_ID             int not null auto_increment,
   COLLECTION_ID        int,
   MOVIE_TRAKT_ID       int not null,
   MOVIE_IMDB_ID        varchar(25) not null,
   MOVIE_TMDB_ID        int not null,
   MOVIE_TITLE          varchar(500) not null,
   MOVIE_ORIGINAL_TITLE varchar(500),
   MOVIE_YEAR           int not null,
   MOVIE_ORIGINAL_LANGUAGE varchar(50),
   MOVIE_OVERVIEW       longtext not null,
   MOVIE_TAGLINE        varchar(1000),
   MOVIE_POPULARITY     decimal not null,
   MOVIE_RELEASE_DATE   date,
   MOVIE_VOTE_AVERAGE   decimal not null,
   MOVIE_VOTE_COUNT     int not null,
   MOVIE_RUNTIME        int,
   MOVIE_BACKDROP_PATH  varchar(1000),
   MOVIE_POSTER_PATH    varchar(1000),
   MOVIE_CREATED_AT     date not null,
   primary key (MOVIE_ID)
);

/*==============================================================*/
/* Table : MOVIE_GENRE                                          */
/*==============================================================*/
create table MOVIE_GENRE
(
   GENRE_ID             int not null,
   MOVIE_ID             int not null,
   primary key (GENRE_ID, MOVIE_ID)
);

/*==============================================================*/
/* Table : PERSON                                               */
/*==============================================================*/
create table PERSON
(
   PERSON_ID            int not null auto_increment,
   PERSON_FIRSTNAME     varchar(250) not null,
   PERSON_LASTNAME      varchar(250) not null,
   PERSON_PROFILE_PATH  varchar(1000),
   PERSON_CREATED_AT    date not null,
   primary key (PERSON_ID)
);

/*==============================================================*/
/* Table : QUALITY                                              */
/*==============================================================*/
create table QUALITY
(
   QUALITY_ID           int not null auto_increment,
   QUALITY_NAME         varchar(50) not null,
   QUALITY_CREATED_AT   date not null,
   primary key (QUALITY_ID)
);

/*==============================================================*/
/* Table : TORRENT                                              */
/*==============================================================*/
create table TORRENT
(
   TORRENT_ID           int not null auto_increment,
   MOVIE_ID             int,
   CATEGORY_ID          int not null,
   LANGUAGE_ID          int not null,
   QUALITY_ID           int not null,
   TORRENT_T411_ID      int not null,
   TORRENT_NAME         varchar(250) not null,
   TORRENT_SLUG         varchar(250),
   TORRENT_YEAR         int,
   TORRENT_SIZE         bigint not null,
   TORRENT_SEEDERS      int,
   TORRENT_LEECHERS     int,
   TORRENT_COMPLETED    int,
   TORRENT_CREATED_AT   date not null,
   primary key (TORRENT_ID)
);

alter table CHARACTER add constraint FK_CHARACTER foreign key (MOVIE_ID)
      references MOVIE (MOVIE_ID) on delete restrict on update restrict;

alter table CHARACTER add constraint FK_CHARACTER2 foreign key (PERSON_ID)
      references PERSON (PERSON_ID) on delete restrict on update restrict;

alter table MOVIE add constraint FK_MOVIE_COLLECTION foreign key (COLLECTION_ID)
      references COLLECTION (COLLECTION_ID) on delete restrict on update restrict;

alter table MOVIE_GENRE add constraint FK_MOVIE_GENRE foreign key (GENRE_ID)
      references GENRE (GENRE_ID) on delete restrict on update restrict;

alter table MOVIE_GENRE add constraint FK_MOVIE_GENRE2 foreign key (MOVIE_ID)
      references MOVIE (MOVIE_ID) on delete restrict on update restrict;

alter table TORRENT add constraint FK_MOVIE_TORRENT foreign key (MOVIE_ID)
      references MOVIE (MOVIE_ID) on delete restrict on update restrict;

alter table TORRENT add constraint FK_TORRENT_CATEGORY foreign key (CATEGORY_ID)
      references CATEGORY (CATEGORY_ID) on delete restrict on update restrict;

alter table TORRENT add constraint FK_TORRENT_LANGUAGE foreign key (LANGUAGE_ID)
      references LANGUAGE (LANGUAGE_ID) on delete restrict on update restrict;

alter table TORRENT add constraint FK_TORRENT_QUALITY foreign key (QUALITY_ID)
      references QUALITY (QUALITY_ID) on delete restrict on update restrict;

