--select * from Products
--delete from Products
/*
declare @i int = 0
while @i < 100
begin
	insert into Products(Name, Category, Description, Price) values('test' + convert(varchar(3), @i), 'test1', 'test1', '666')
	set @i += 1
end
