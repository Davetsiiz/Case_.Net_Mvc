create database Futbolig_Crawler
use Futbolig_Crawler

create table Teams (ID int not null identity primary key, Name varchar(30),Logo varchar(max),Coach varchar(50))
create table Player(ID int not null identity primary key,TeamId int not null foreign key references Teams(ID),Photo varchar(max),Age int)
alter table player add Name varchar(50)


