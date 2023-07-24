
create table Template
(
	TemplateID int primary key,
	CompID tinyint constraint FK_Template_Company  foreign key (CompID) references tblCompany (CompID),
	Name varchar(100) not null,
	Short varchar(10) null,
	IsBankEnabled bit not null default(0),
	IsActive bit not null,
	CreatedOn datetime,
	CreatedBy int,
	ModifiedOn datetime,
	ModifiedBy int
)

go

create table TemplateDetail
(
	TemplateDetailID int primary key identity,
	TemplateID int constraint FK_TemplateDetail_Template  foreign key (TemplateID) references Template (TemplateID),
	Account varchar(12) not null,
	Behaviour varchar(2) not null,
	IsSingle bit not null,
	IsActive bit not null,
	CreatedOn datetime,
	CreatedBy int,
	ModifiedOn datetime,
	ModifiedBy int
)

go
create table TemplateHeadAccount
(
	TemplateHeadAccountID int primary key identity,
	TemplateID int constraint FK_TemplateHeadAccount_Template foreign key (TemplateID) references Template (TemplateID),
	Account varchar(12),
	IsActive bit
)

go

delete tblAppPrivilage where AmID between 375 and 389
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
go



insert into Template values 
(61, 1, 'SDA to HO Voucher', 'SDAHOV', 1, 1, null, null, null, null),
(62, 1, 'SDA to Branch Voucher', 'SDABRV', 1, 1, null, null, null, null),
(63, 1, 'SDA to Actual Expense', 'SDAAEV', 1, 1, null, null, null, null),
(64, 1, 'Bank Payment Voucher', 'BPBRV', 1, 1, null, null, null, null),
(65, 1, 'Bank Payment HO to Branch', 'BPHOV', 1, 1, null, null, null, null),
(66, 1, 'Cash Payment Voucher', 'CPBRV', 0, 1, null, null, null, null),
(67, 1, 'SDA Voucher', 'SDARV', 1, 1, null, null, null, null),
(68, 1, 'Branch to HO Voucher',  'BRHOV', 1, 1, null, null, null, null),
(69, 1, 'Direct Income Voucher', 'BRDIV', 1, 1, null, null, null, null),
(70, 1, 'Cash Receipt Voucher', 'BRCRV', 0, 1, null, null, null, null)


go
--test
insert into TemplateDetail values 
(61, '0101010019', 'Dr', 1, 1, null, null, null, null),
(61, '0101010018', 'Cr', 0, 1, null, null, null, null)--don't create default show empty

--final
insert into TemplateDetail values 
(63, '0101010010', 'Dr', 1, 1, null, null, null, null),--218--development--55 non development
(63, '0501010001', 'Cr', 0, 1, null, null, null, null)--sbg--don't create default show empty

--final
insert into TemplateDetail values 
(64, '0101010011', 'Dr', 1, 1, null, null, null, null),--bpv
(64, '0502010001', 'Cr', 0, 1, null, null, null, null)--accounts tel

--final
insert into TemplateDetail values 
(64, '0101010011', 'Dr', 1, 1, null, null, null, null),--bpv
(64, '0502010001', 'Cr', 0, 1, null, null, null, null)--accounts tel


go



insert into Vr_Type values 
(61, 'SDAtoHOVoucher', 'PU', 61),
(62, 'SDAtoBranchVoucher', 'PU', 62),
(63, 'SDAtoActualExpense', 'PU', 63),
(64, 'BankPaymentVoucher', 'PU', 64),
(65, 'BankHOtoBranch', 'PU', 65),
(66, 'CashPaymentVoucher', 'PU', 66),
(67, 'SDAVoucher', 'PU', 67),
(68, 'BranchtoHOVoucher',  'PU', 68),
(69, 'DirectIncomeVoucher', 'PU', 69),
(70, 'CashReceiptVoucher', 'PU', 70)

go




/*
[dbo].[spTestGL] 7755
*/
ALTER PROCEDURE [dbo].[spTestGL] (@vrid int)
AS
BEGIN

declare @templateId int, @singleAcccount varchar(20)

select @templateId = templateId from template where templateId in (select vt_cd from glmf_data where vrid = @vrid)
select @singleAcccount = account from templateDetail where templateId = @templateId and isSingle = 1 and IsActive = 1


SELECT     det.vr_seq, det.gl_cd, code.gl_dsc, det.vrd_debit, det.vrd_credit, 
           Cost.cc_nme,det.vrd_nrtn, det.cc_cd, data.vr_dt, data.vrid,data.vr_nrtn,
           data.vr_no,vtype.vt_use,
           isnull(uzrupd.UserName,'') updateby, data.updateon,
           isnull(uzrapr.UserName,'') approvedby, data.approvedon,
           data.source, data.Ref_no, data.vt_cd
FROM       glmf_data data INNER JOIN
                 glmf_data_det  det ON data.vrid = det.vrid INNER JOIN
                glmf_code code ON det.gl_cd = code.gl_cd
                      LEFT JOIN Cost_Center Cost ON det.cc_cd = Cost.cc_cd
                      LEFT JOIN vr_type vtype ON data.vt_cd = vtype.vt_cd
                      left join tblAppUser uzrapr on uzrapr.LoginID = data.approvedby
                      left join tblAppUser uzrupd on uzrupd.LoginID = data.updateby
WHERE     (data.vrid = @vrid and det.gl_cd != @singleAcccount)
END






go




create table tblPlEmpEdu
(
	EmpEduID int primary key identity,
		EmpID int constraint FK_EmpEdu_Emp  foreign key (EmpID) references tblPlEmpData (EmpID),
	CityID int constraint FK_EmpEdu_City  foreign key (CityID) references tblCity (CityID),
	DegreeTitle varchar(200),
	UniversityBoard varchar(200),
	Year decimal(4, 0),
	Verified bit default(0)
)