create database econophysics;
use econophysics;
create table Parameters
(
Exp int,
Maxturn int not null,
PeriodOfTurn int not null,
Leverage bool not null,
Lambda double not null,
P01 double not null,
P10 double not null,
TradeFee double not null,
P double not null,
TransP double not null,
Datetime datetime not null,
Comments text(200),
primary key (Exp)
);
create table Market
(
Exp int,
Turn int,
Price double not null,
State bool not null,
Returns int not null,
primary key(Exp,Turn),
foreign key (Exp) references Parameters(Exp) on delete cascade on update cascade
);
create table Agents
(
Id int,
Turn int,
Exp int,
Cash double not null,
Stocks int not null,
Dividend double not null,
Tradestocks int not null,
Endowment double not null,
primary key (Id,Turn,Exp),
foreign key (Exp) references Parameters(Exp) on delete cascade on update cascade
);
insert into mysql.user values("localhost","agent",password("1234"));
insert into mysql.user values("localhost","market",password("1234"));
insert into mysql.user values("localhost","admin",password("1234"));

