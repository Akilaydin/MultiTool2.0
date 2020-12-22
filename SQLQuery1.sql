Create database MultiNotes
use MultiNotes

create table Notes (
Note_id int identity primary key,
Note_title nvarchar(255) unique,
Note_text text,
Note_tag nvarchar(255),
Note_priority int,
Note_date datetime
);

alter table Notes add Note_edit_date datetime null

ALTER TABLE Notes  
    ADD CONSTRAINT note_date_default  
    DEFAULT CURRENT_TIMESTAMP FOR Note_date;  

insert into Notes (Note_title, Note_text, Note_tag, Note_priority) values ('Инструкция','Для создания заметки нажмите на изображение карандаша в левом нижнем углу.' + CHAR(13)+CHAR(10) + 'Для удаления заметки, выделите соответствующую заметку в списке слева и нажмите на изображение с корзиной в нижнем левом углу','Инструкция',3 );


select * from Notes
drop table Notes


insert into Persons (ID,LastName,FirstName,Age) values (1,'assa','as',15)
select * from Persons