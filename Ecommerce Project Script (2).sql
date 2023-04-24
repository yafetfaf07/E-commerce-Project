create database ecommerceproject
use ecommerceproject



--creating tables
create table buyer(
buyerid int identity primary key not null,
buyerfirstname varchar(50) not null,
buyerlastname varchar(50) not null,
buyercreditcard varchar(50) not null,
username varchar(50) unique not null,
[password] varchar(50) not null,
email varchar(50) not null
)

create table seller(
sellerid int identity primary key not null,
sellerfirstname varchar(50) not null,
sellerlastname varchar(50) not null, 
sellercreditcard varchar(50)not null,
sellerrevenue float not null default 0.0,
username varchar(50) unique not null,
[password] varchar(50) not null,
email varchar(50) not null
)

create table product (
productid int identity primary key not null,
productname varchar(100) not null,
productcategory varchar(50),
productprice float not null,
productimage image not null,
productquantity int not null,
sellerid int not null
)

create table bought(
buyerid int,
productid int)

create table cart(
buyerid int,
productid int)



--table constraints
alter table product 
add constraint fk_prodsellerid
foreign key(sellerid) references seller(sellerid)

alter table bought 
add constraint fk_buyeridbuy
foreign key(buyerid) references buyer(buyerid)

alter table bought 
add constraint fk_productidbuy
foreign key(productid) references product(productid)

alter table cart
add constraint fk_buyeridcart
foreign key(buyerid) references buyer(buyerid)

alter table cart 
add constraint fk_productidcart
foreign key(productid) references product(productid)




--procedures
go
create proc sp_signupseller 
@sellerfirstname varchar(20), @sellerlastname varchar(20),
@sellercreditcard varchar(19), @username varchar(30),  
@password varchar(30), @email varchar(30)  
as  
begin  
insert into seller (sellerfirstname, sellerlastname, sellercreditcard, username, [password], email)
values(
@sellerfirstname, @sellerlastname , @sellercreditcard , @username , @password , @email   
)  
end


go 
create proc sp_signupbuyer
@buyerfirstname varchar(20), @buyerlastname varchar(20), @buyercreditcard varchar(19),
@username varchar(30), @password varchar(30), @email varchar(30)
as
begin
insert into buyer values(
@buyerfirstname , @buyerlastname , @buyercreditcard ,@username ,
@password ,@email 
)
end


go
create proc sp_addproduct
@productname varchar(100), @productcategory varchar(50), @productprice float,
@productimage image, @productquantity int, @sellerid int
as
begin
Insert into product values (
@productname, @productcategory, @productprice, @productimage ,@productquantity, @sellerid)
end


go
create proc sp_updatebuyer
@fname varchar(50), @lname varchar(50), @creditcard varchar(50), @username varchar(50),
@password varchar(50), @email varchar(50), @buyerid int
as
begin
update buyer set buyerfirstname = @fname, buyerlastname = @lname, buyercreditcard = @creditcard,
username = @username, [password] = @password, email = @email where buyerid = @buyerid
end


go
create proc sp_updateseller
@fname varchar(50), @lname varchar(50), @creditcard varchar(50), @username varchar(50),
@password varchar(50), @email varchar(50), @sellerid int
as
begin
update seller set sellerfirstname = @fname, sellerlastname = @lname, sellercreditcard = @creditcard,
username = @username, [password] = @password, email = @email where sellerid = @sellerid
end


go
create proc sp_updateproduct
@productname varchar(100), @category varchar(50), @price float, @image image, @quantity int, @prodid int
as
begin
update product set productname = @productname, productcategory = @category, productprice = @price,
productimage = @image, productquantity = @quantity where productid = @prodid
end


select * from buyer
select *from product
select * from seller

--update seller set sellerrevenue=0

-- For the BUY Button
go
create proc buy_update_product (@productId int, @revenue float, @sellerid int)
as
begin
declare @prodquantities int

	begin transaction 
	update product set productquantity = productquantity-1 where productid = @productId
	update seller set sellerrevenue = @revenue where sellerid = @sellerid
	select @prodquantities = (select productquantity from product where productid=@productId)
	if (@prodquantities<0)
		begin
		rollback 
		
		end
	else
		begin
			commit 
			
		end
	
end

drop proc buy_update_product


--Views created for the Homepage
go
create view [productDescription] as
select * from product

go
create view [productcategoryClothing] as
select * from product where productcategory='Clothing'

go
create view [productcategoryBooks] as
select * from product where productcategory='Books'

go
create view [productcategoryother] as
select * from product where productcategory='Other'

go
create view [productcategoryElectronics] as
select * from product where productcategory='Electronics'





