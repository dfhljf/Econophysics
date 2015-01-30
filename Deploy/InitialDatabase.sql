create database econophysics;
use econophysics;
create table Parameters
(
Id int,
MaxStock int not null,
PeriodOfUpdateDividend int not null,
TradeFee double not null,
Count int not null,
Leverage int not null,
Lambda double not null,
P01 double not null,
P10 double not null,
PDividend double not null,
P double not null,
TransP double not null,
TimeWindow int not null,
PeriodOfTurn int not null,
MaxTurn int not null,
DateTime datetime not null,
Comments text(200),
primary key (Id)
);
create table Market
(
ExperimentId int,
Turn int,
Price double not null,
State int not null,
Returns int not null,
NumberOfPeople int not null,
AverageEndowment double not null,
Volume int not null,
primary key(ExperimentId,Turn),
foreign key (ExperimentId) references Parameters(Id) on delete cascade on update cascade
);
create table Agents
(
ExperimentId int,
Turn int,
Id int,
Cash double not null,
Stocks int not null,
Endowment double not null,
Dividend double not null,
TradeStocks int not null,
OrderNum int not null,
primary key (ExperimentId,Turn,Id),
foreign key (ExperimentId) references Parameters(Id) on delete cascade on update cascade
);
create user 'agent'@'localhost' identified by '123456';
grant insert on econophysics.agents to 'agent'@'localhost';

create user 'market'@'localhost' identified by '123456';
grant insert on econophysics.market to 'market'@'localhost';

create user 'experiment'@'localhost' identified by '123456';
grant select,insert on econophysics.* to 'experiment'@'localhost';