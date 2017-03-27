select m.Name,p.Name
from ((movieActors ma join Movies m on ma.Movie_id=m.Id)join actors a on ma.Actor_Id=a.id)join people p on p.id = a.personId  ;
select * from people

select * from people
where Name like '%Kevin%' 
select *from movies
select * from actors
select * from movieactors