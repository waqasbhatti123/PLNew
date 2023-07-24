


update tblAppMenu set Enabled= 0 where AmIDParent in (321,
322,
323,
324,
325)
go
update tblAppPrivilage set Enabled= 0 where AmID in (321,
322,
323,
324,
325)
go

insert into tblAppMenu values (375, 'Payment Vouchers', null, '', '', 1, 3, 375)

insert into tblAppMenu values (376, 'SDA to HO Voucher', 375, '../glsetup/voucher.aspx', '', 1, 3, 376)
insert into tblAppMenu values (377, 'SDA to Branch Voucher', 375, '../glsetup/voucher.aspx', '', 1, 3, 377)
insert into tblAppMenu values (378, 'SDA to Actual Expense', 375, '../glsetup/voucher.aspx', '', 1, 3, 378)
insert into tblAppMenu values (379, 'Cash Payment Voucher ', 375, '../glsetup/voucher.aspx', '', 1, 3, 379)
insert into tblAppMenu values (380, 'Bank Payment Voucher ', 375, '../glsetup/voucher.aspx', '', 1, 3, 380)
insert into tblAppMenu values (381, 'Bank Payment HO to Branch', 375, '../glsetup/voucher.aspx', '', 1, 3, 381)

go

insert into tblAppMenu values (385, 'Receipt Vouchers', null, '', '', 1, 3, 385)
insert into tblAppMenu values (386, 'SDA Voucher', 385, '../glsetup/voucher.aspx', '', 1, 3, 386)
insert into tblAppMenu values (387, 'Branch to HO Voucher', 385, '../glsetup/voucher.aspx', '', 1, 3, 387)
insert into tblAppMenu values (388, 'Direct Income Voucher', 385, '../glsetup/voucher.aspx', '', 1, 3, 388)
insert into tblAppMenu values (389, 'Cash Receipt Voucher', 385, '../glsetup/voucher.aspx', '', 1, 3, 389)



go




insert into tblAppPrivilage 
select AmID, 1, 1, 1, 1, 1, 1 from tblAppMenu where AmID between 375 and 389
go
insert into tblAppPrivilage 
select AmID, 2, 1, 1, 1, 1, 1 from tblAppMenu where AmID between 375 and 389
go
insert into tblAppPrivilage 
select AmID, 3, 1, 1, 1, 1, 1 from tblAppMenu where AmID between 375 and 389
go
insert into tblAppPrivilage 
select AmID, 4, 1, 1, 1, 1, 1 from tblAppMenu where AmID between 375 and 389
go
insert into tblAppPrivilage 
select AmID, 6, 1, 1, 1, 1, 1 from tblAppMenu where AmID between 375 and 389
go
insert into tblAppPrivilage 
select AmID, 7, 1, 1, 1, 1, 1 from tblAppMenu where AmID between 375 and 389
go
insert into tblAppPrivilage 
select AmID, 8, 1, 1, 1, 1, 1 from tblAppMenu where AmID between 375 and 389
go
insert into tblAppPrivilage 
select AmID, 9, 1, 1, 1, 1, 1 from tblAppMenu where AmID between 375 and 389
go
insert into tblAppPrivilage 
select AmID, 10, 1, 1, 1, 1, 1 from tblAppMenu where AmID between 375 and 389


