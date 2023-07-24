<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="GrantRelease.aspx.cs" Inherits="RMS.GLSetup.GrantRelease"
    Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script>
        $(document).ready(function () {


            $(".click").click(function () {
                window.open("GrantReleaseReport.aspx");
                location.reload();
            });

           
            var brID = '<%=Session["BranchID"].ToString() %>';
            if (brID == "1") {
                $('#btnSave').prop('disabled', false);
                $(".click").show();
            }
            else {
                $('#btnSave').prop('disabled', true);
                $(".click").hide();
            }
            getBudget(brID);
            $('.searchbranchchange').val(brID);

            $('.searchbranchchange').change(function () {

                var Br_ID = $('.searchbranchchange').val();
                getBudget(Br_ID);

            });



            $('#btnSave').click(function () {
                var accounts = [];

                var branchId = $('.searchbranchchange').val();
                $('.clAccount').each(function (index, item) {
                    var accountId = $(this).attr('accountId');
                    var $tr = $(this).closest('tr');
                    var grantid = $tr.find('.grant').val();
                    var grantQ1 = $tr.find('.txtQuarter1').val();
                    var grantQ2 = $tr.find('.txtQuarter2').val();
                    var grantQ3 = $tr.find('.txtQuarter3').val();
                    var grantQ4 = $tr.find('.txtQuarter4').val();
                    var firstExces = $tr.find('.txtfirstexcess').val();
                    var firstSurrend = $tr.find('.txtfirstsurrender').val();
                    var SecondExces = $tr.find('.txtsecondexcess').val();
                    var Secondsurren = $tr.find('.txtsecondsurrender').val();
                    var FirstApp = $tr.find('.txtfirstapp').val();
                    var Secondapp = $tr.find('.txtsecondapp').val();
                    var thirdapp = $tr.find('.txtthirdapp').val();
                    var forthapp = $tr.find('.txtfourthapp').val();
                    var perposedid = $tr.find('.perposed').val();
                    var push = accounts.push({
                        Account: accountId, Grant: grantid, GrantinQ1: grantQ1, GrantinQ2: grantQ2, GrantinQ3: grantQ3, GrantinQ4: grantQ4,
                        Aid: perposedid, br_id: branchId, firstExcess: firstExces, firstSurrender: firstSurrend, secondExcess: SecondExces,
                        secondSurrender : Secondsurren ,firstapprop: FirstApp, secondapprop: Secondapp,
                        thirdapprop : thirdapp, forthapprop : forthapp
                    });
                    //if (push) {
                    //    var $tr = $(this).closest('tr');
                    //var Incomeid = $tr.find('.income').val("");
                    //var grantid = $tr.find('.grant').val("");
                    //var perposedid = $tr.find('.perposed').val("");
                    //}
                })
                            

            $.ajax({
                url: "GrantRelease.aspx/SaveBudget",
                data: JSON.stringify({ budget: accounts }),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    window.alert(data.d);
                    
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
            });
            $('table').on('change', '.txtQuarter1', function () {
                    var $tr = $(this).closest('tr');
                
                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                 var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                 var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                 var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }
                var firstexxx = $tr.find('.txtfirstexcess').val();
                if (firstexxx == "" || firstexxx == null) {
                    firstexxx = 0;
                }
                var firstsurr = $tr.find('.txtfirstsurrender').val();
                if (firstsurr == "" || firstsurr == null) {
                    firstsurr = 0;
                }
                var secondexx = $tr.find('.txtsecondexcess').val();
                if (secondexx == "" || secondexx == null) {
                    secondexx = 0;
                }
                var secondsurr = $tr.find('.txtsecondsurrender').val();
                if (secondsurr == "" || secondsurr == null) {
                    secondsurr = 0;
                }
                var firstappp = $tr.find('.txtfirstapp').val();
                if (firstappp == "" || firstappp == null) {
                    firstappp = 0;
                }
                var secondappp = $tr.find('.txtsecondapp').val();
                if (secondappp == "" || secondappp == null) {
                    secondappp = 0;
                }
                var thirdapp = $tr.find('.txtthirdapp').val();
                if (thirdapp == "" || thirdapp == null) {
                    thirdapp = 0;
                }
                var forthapp = $tr.find('.txtfourthapp').val();
                if (forthapp == "" || forthapp == null) {
                    forthapp = 0;
                }
                var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(firstexxx) - parseInt(firstsurr) + parseInt(secondexx) -
                    parseInt(secondsurr) + parseInt(firstappp)
                    + parseInt(secondappp) + parseInt(thirdapp) + parseInt(forthapp);

                $tr.find('.perposed').val(total);

                    getQ1Total();
                 
                getGrandTotal();
                onloadTotalvar();

            });
             $('table').on('keyup', '.txtQuarter2', function () {
                    var $tr = $(this).closest('tr');
                
                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                 var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                 var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                 var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }
                   var firstexxx = $tr.find('.txtfirstexcess').val();
                if (firstexxx == "" || firstexxx == null) {
                    firstexxx = 0;
                 }
                  var firstsurr = $tr.find('.txtfirstsurrender').val();
                if (firstsurr == "" || firstsurr == null) {
                    firstsurr = 0;
                }
                var secondsurr = $tr.find('.txtsecondsurrender').val();
                if (secondsurr == "" || secondsurr == null) {
                    secondsurr = 0;
                 }
                 var secondexx = $tr.find('.txtsecondexcess').val();
                if (secondexx == "" || secondexx == null) {
                    secondexx = 0;
                }
                var firstappp = $tr.find('.txtfirstapp').val();
                if (firstappp == "" || firstappp == null) {
                    firstappp = 0;
                }
                var secondappp = $tr.find('.txtsecondapp').val();
                if (secondappp == "" || secondappp == null) {
                    secondappp = 0;
                }
                var thirdapp = $tr.find('.txtthirdapp').val();
                if (thirdapp == "" || thirdapp == null) {
                    thirdapp = 0;
                }
                var forthapp = $tr.find('.txtfourthapp').val();
                if (forthapp == "" || forthapp == null) {
                    forthapp = 0;
                }


                 var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(firstexxx) - parseInt(firstsurr) - parseInt(secondsurr) + parseInt(secondexx) + parseInt(firstappp)
                    + parseInt(secondappp) + parseInt(thirdapp) + parseInt(forthapp) ;

                 $tr.find('.perposed').val(total);


                    getQ2Total();
                 
                 getGrandTotal();
                 onloadTotalvar();

            });
             $('table').on('keyup', '.txtQuarter3', function () {
                    var $tr = $(this).closest('tr');
                
                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                 var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                 var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                 var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }
                   var firstexxx = $tr.find('.txtfirstexcess').val();
                if (firstexxx == "" || firstexxx == null) {
                    firstexxx = 0;
                 }
                 var firstsurr = $tr.find('.txtfirstsurrender').val();
                if (firstsurr == "" || firstsurr == null) {
                    firstsurr = 0;
                }
                var secondexx = $tr.find('.txtsecondexcess').val();
                if (secondexx == "" || secondexx == null) {
                    secondexx = 0;
                 }
                 var secondsurr = $tr.find('.txtsecondsurrender').val();
                if (secondsurr == "" || secondsurr == null) {
                    secondsurr = 0;
                }
                var firstappp = $tr.find('.txtfirstapp').val();
                if (firstappp == "" || firstappp == null) {
                    firstappp = 0;
                }
                var secondappp = $tr.find('.txtsecondapp').val();
                if (secondappp == "" || secondappp == null) {
                    secondappp = 0;
                }
                var thirdapp = $tr.find('.txtthirdapp').val();
                if (thirdapp == "" || thirdapp == null) {
                    thirdapp = 0;
                }
                var forthapp = $tr.find('.txtfourthapp').val();
                if (forthapp == "" || forthapp == null) {
                    forthapp = 0;
                }



                 var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(firstexxx) - parseInt(firstsurr)
                     + parseInt(secondexx) - parseInt(secondsurr) + parseInt(firstappp)
                    + parseInt(secondappp) + parseInt(thirdapp) + parseInt(forthapp) ;

                 $tr.find('.perposed').val(total);

                    getQ3Total();
                 
                 getGrandTotal();
                 onloadTotalvar();

            });
             $('table').on('keyup', '.txtQuarter4', function () {
                    var $tr = $(this).closest('tr');
                
                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                 var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                 var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                 var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }

                   var firstexxx = $tr.find('.txtfirstexcess').val();
                if (firstexxx == "" || firstexxx == null) {
                    firstexxx = 0;
                 }
                 var firstsurr = $tr.find('.txtfirstsurrender').val();
                if (firstsurr == "" || firstsurr == null) {
                    firstsurr = 0;
                }
                var secondexx = $tr.find('.txtsecondexcess').val();
                if (secondexx == "" || secondexx == null) {
                    secondexx = 0;
                 }
                 var secondsurr = $tr.find('.txtsecondsurrender').val();
                if (secondsurr == "" || secondsurr == null) {
                    secondsurr = 0;
                }
                var firstappp = $tr.find('.txtfirstapp').val();
                if (firstappp == "" || firstappp == null) {
                    firstappp = 0;
                }
                var secondappp = $tr.find('.txtsecondapp').val();
                if (secondappp == "" || secondappp == null) {
                    secondappp = 0;
                }
                var thirdapp = $tr.find('.txtthirdapp').val();
                if (thirdapp == "" || thirdapp == null) {
                    thirdapp = 0;
                }
                var forthapp = $tr.find('.txtfourthapp').val();
                if (forthapp == "" || forthapp == null) {
                    forthapp = 0;
                }



                 var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(firstexxx) - parseInt(firstsurr) +
                     parseInt(secondexx) - parseInt(secondsurr) + parseInt(firstappp)
                    + parseInt(secondappp) + parseInt(thirdapp) + parseInt(forthapp);

                 $tr.find('.perposed').val(total);

                    getQ4Total();
                 
                 getGrandTotal();
                 onloadTotalvar();

            });
            $('table').on('keyup', '.txtfirstexcess', function () {
                    var $tr = $(this).closest('tr');
                
                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                 var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                 var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                 var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }

                   var firstexxx = $tr.find('.txtfirstexcess').val();
                if (firstexxx == "" || firstexxx == null) {
                    firstexxx = 0;
                }
                 var firstsurr = $tr.find('.txtfirstsurrender').val();
                if (firstsurr == "" || firstsurr == null) {
                    firstsurr = 0;
                }
                var secondexx = $tr.find('.txtsecondexcess').val();
                if (secondexx == "" || secondexx == null) {
                    secondexx = 0;
                }
                var secondsurr = $tr.find('.txtsecondsurrender').val();
                if (secondsurr == "" || secondsurr == null) {
                    secondsurr = 0;
                }
                var firstappp = $tr.find('.txtfirstapp').val();
                if (firstappp == "" || firstappp == null) {
                    firstappp = 0;
                }
                var secondappp = $tr.find('.txtsecondapp').val();
                if (secondappp == "" || secondappp == null) {
                    secondappp = 0;
                }
                var thirdapp = $tr.find('.txtthirdapp').val();
                if (thirdapp == "" || thirdapp == null) {
                    thirdapp = 0;
                }
                var forthapp = $tr.find('.txtfourthapp').val();
                if (forthapp == "" || forthapp == null) {
                    forthapp = 0;
                }



                var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(firstexxx) - parseInt(firstsurr) + parseInt(secondexx) -
                    parseInt(secondsurr) + parseInt(firstappp)
                    + parseInt(secondappp) + parseInt(thirdapp) + parseInt(forthapp);

                 $tr.find('.perposed').val(total);

                getfirstexcessTotal();
                 
                 getGrandTotal();
                 onloadTotalvar();

            });
            $('table').on('keyup', '.txtsecondexcess', function () {
                    var $tr = $(this).closest('tr');
                
                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                 var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                 var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                 var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }

                   var firstexxx = $tr.find('.txtfirstexcess').val();
                if (firstexxx == "" || firstexxx == null) {
                    firstexxx = 0;
                }
                 var firstsurr = $tr.find('.txtfirstsurrender').val();
                if (firstsurr == "" || firstsurr == null) {
                    firstsurr = 0;
                }
                var secondexx = $tr.find('.txtsecondexcess').val();
                if (secondexx == "" || secondexx == null) {
                    secondexx = 0;
                }
                var secondsurr = $tr.find('.txtsecondsurrender').val();
                if (secondsurr == "" || secondsurr == null) {
                    secondsurr = 0;
                }
                var firstappp = $tr.find('.txtfirstapp').val();
                if (firstappp == "" || firstappp == null) {
                    firstappp = 0;
                }
                var secondappp = $tr.find('.txtsecondapp').val();
                if (secondappp == "" || secondappp == null) {
                    secondappp = 0;
                }
                var thirdapp = $tr.find('.txtthirdapp').val();
                if (thirdapp == "" || thirdapp == null) {
                    thirdapp = 0;
                }
                var forthapp = $tr.find('.txtfourthapp').val();
                if (forthapp == "" || forthapp == null) {
                    forthapp = 0;
                }



                var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(firstexxx) - parseInt(firstsurr) + parseInt(secondexx) -
                    parseInt(secondsurr) + parseInt(firstappp)
                    + parseInt(secondappp) + parseInt(thirdapp) + parseInt(forthapp);

                 $tr.find('.perposed').val(total);

                getsecondexcessTotal();
                 
                 getGrandTotal();
                 onloadTotalvar();

            });
            $('table').on('keyup', '.txtfirstapp', function () {
                    var $tr = $(this).closest('tr');
                
                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                 var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                 var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                 var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }

                   var firstexxx = $tr.find('.txtfirstexcess').val();
                if (firstexxx == "" || firstexxx == null) {
                    firstexxx = 0;
                }

                var firstsurr = $tr.find('.txtfirstsurrender').val();
                if (firstsurr == "" || firstsurr == null) {
                    firstsurr = 0;
                }

                var secondexx = $tr.find('.txtsecondexcess').val();
                if (secondexx == "" || secondexx == null) {
                    secondexx = 0;
                }
                var secondsurr = $tr.find('.txtsecondsurrender').val();
                if (secondsurr == "" || secondsurr == null) {
                    secondsurr = 0;
                }
                var firstappp = $tr.find('.txtfirstapp').val();
                if (firstappp == "" || firstappp == null) {
                    firstappp = 0;
                }
                var secondappp = $tr.find('.txtsecondapp').val();
                if (secondappp == "" || secondappp == null) {
                    secondappp = 0;
                }
                var thirdapp = $tr.find('.txtthirdapp').val();
                if (thirdapp == "" || thirdapp == null) {
                    thirdapp = 0;
                }
                var forthapp = $tr.find('.txtfourthapp').val();
                if (forthapp == "" || forthapp == null) {
                    forthapp = 0;
                }



                var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(firstexxx) - parseInt(firstsurr) +
                     parseInt(secondexx) - parseInt(secondsurr) + parseInt(firstappp)
                    + parseInt(secondappp) + parseInt(thirdapp) + parseInt(forthapp);

                 $tr.find('.perposed').val(total);

                getfirstappTotal();
                 
                 getGrandTotal();
                 onloadTotalvar();

            });
            $('table').on('keyup', '.txtsecondapp', function () {
                    var $tr = $(this).closest('tr');
                
                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                 var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                 var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                 var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }

                   var firstexxx = $tr.find('.txtfirstexcess').val();
                if (firstexxx == "" || firstexxx == null) {
                    firstexxx = 0;
                }
                var firstsurr = $tr.find('.txtfirstsurrender').val();
                if (firstsurr == "" || firstsurr == null) {
                    firstsurr = 0;
                }
                var secondexx = $tr.find('.txtsecondexcess').val();
                if (secondexx == "" || secondexx == null) {
                    secondexx = 0;
                }
                var secondsurr = $tr.find('.txtsecondsurrender').val();
                if (secondsurr == "" || secondsurr == null) {
                    secondsurr = 0;
                }
                var firstappp = $tr.find('.txtfirstapp').val();
                if (firstappp == "" || firstappp == null) {
                    firstappp = 0;
                }
                var secondappp = $tr.find('.txtsecondapp').val();
                if (secondappp == "" || secondappp == null) {
                    secondappp = 0;
                }
                var thirdapp = $tr.find('.txtthirdapp').val();
                if (thirdapp == "" || thirdapp == null) {
                    thirdapp = 0;
                }
                var forthapp = $tr.find('.txtfourthapp').val();
                if (forthapp == "" || forthapp == null) {
                    forthapp = 0;
                }



                var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(firstexxx)
                    - parseInt(firstsurr) + parseInt(secondexx) - parseInt(secondsurr) + parseInt(firstappp)
                    + parseInt(secondappp) + parseInt(thirdapp) + parseInt(forthapp);

                 $tr.find('.perposed').val(total);

                getsecondappTotal();
                 
                 getGrandTotal();
                 onloadTotalvar();

            });
            $('table').on('keyup', '.txtthirdapp', function () {
                    var $tr = $(this).closest('tr');
                
                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                 var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                 var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                 var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }

                   var firstexxx = $tr.find('.txtfirstexcess').val();
                if (firstexxx == "" || firstexxx == null) {
                    firstexxx = 0;
                }

                var firstsurr = $tr.find('.txtfirstsurrender').val();
                if (firstsurr == "" || firstsurr == null) {
                    firstsurr = 0;
                }
                var secondexx = $tr.find('.txtsecondexcess').val();
                if (secondexx == "" || secondexx == null) {
                    secondexx = 0;
                }

                var secondsurr = $tr.find('.txtsecondsurrender').val();
                if (secondsurr == "" || secondsurr == null) {
                    secondsurr = 0;
                }

                var firstappp = $tr.find('.txtfirstapp').val();
                if (firstappp == "" || firstappp == null) {
                    firstappp = 0;
                }
                var secondappp = $tr.find('.txtsecondapp').val();
                if (secondappp == "" || secondappp == null) {
                    secondappp = 0;
                }
                var thirdapp = $tr.find('.txtthirdapp').val();
                if (thirdapp == "" || thirdapp == null) {
                    thirdapp = 0;
                }
                var forthapp = $tr.find('.txtfourthapp').val();
                if (forthapp == "" || forthapp == null) {
                    forthapp = 0;
                }



                var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(firstexxx) - parseInt(firstsurr) + parseInt(secondexx)
                    - parseInt(secondsurr) + parseInt(firstappp)
                    + parseInt(secondappp) + parseInt(thirdapp) + parseInt(forthapp);

                 $tr.find('.perposed').val(total);

                getthirdappTotal();
                 
                 getGrandTotal();
                 onloadTotalvar();

            });
            $('table').on('keyup', '.txtfourthapp', function () {
                    var $tr = $(this).closest('tr');
                
                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                 var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                 var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                 var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }

                   var firstexxx = $tr.find('.txtfirstexcess').val();
                if (firstexxx == "" || firstexxx == null) {
                    firstexxx = 0;
                }

                var firstsurr = $tr.find('.txtfirstsurrender').val();
                if (firstsurr == "" || firstsurr == null) {
                    firstsurr = 0;
                }

                var secondexx = $tr.find('.txtsecondexcess').val();
                if (secondexx == "" || secondexx == null) {
                    secondexx = 0;
                }

                var secondesurr = $tr.find('.txtsecondsurrender').val();
                if (secondesurr == "" || secondesurr == null) {
                    secondesurr = 0;
                }

                var firstappp = $tr.find('.txtfirstapp').val();
                if (firstappp == "" || firstappp == null) {
                    firstappp = 0;
                }
                var secondappp = $tr.find('.txtsecondapp').val();
                if (secondappp == "" || secondappp == null) {
                    secondappp = 0;
                }
                var thirdapp = $tr.find('.txtthirdapp').val();
                if (thirdapp == "" || thirdapp == null) {
                    thirdapp = 0;
                }
                var forthapp = $tr.find('.txtfourthapp').val();
                if (forthapp == "" || forthapp == null) {
                    forthapp = 0;
                }



                var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(firstexxx) - parseInt(firstsurr) + parseInt(secondexx)
                    - parseInt(secondesurr) + parseInt(firstappp)
                    + parseInt(secondappp) + parseInt(thirdapp) + parseInt(forthapp);

                 $tr.find('.perposed').val(total);

                getforthappTotal();
                 
                 getGrandTotal();
                 onloadTotalvar();

            });
            $('table').on('keyup', '.txtfirstsurrender', function () {
                    var $tr = $(this).closest('tr');
                
                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                 var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                 var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                 var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }

                   var firstexxx = $tr.find('.txtfirstexcess').val();
                if (firstexxx == "" || firstexxx == null) {
                    firstexxx = 0;
                }

                var firstsurr = $tr.find('.txtfirstsurrender').val();
                if (firstsurr == "" || firstsurr == null) {
                    firstsurr = 0;
                }

                var secondexx = $tr.find('.txtsecondexcess').val();
                if (secondexx == "" || secondexx == null) {
                    secondexx = 0;
                }

                var secondesurr = $tr.find('.txtsecondsurrender').val();
                if (secondesurr == "" || secondesurr == null) {
                    secondesurr = 0;
                }

                var firstappp = $tr.find('.txtfirstapp').val();
                if (firstappp == "" || firstappp == null) {
                    firstappp = 0;
                }
                var secondappp = $tr.find('.txtsecondapp').val();
                if (secondappp == "" || secondappp == null) {
                    secondappp = 0;
                }
                var thirdapp = $tr.find('.txtthirdapp').val();
                if (thirdapp == "" || thirdapp == null) {
                    thirdapp = 0;
                }
                var forthapp = $tr.find('.txtfourthapp').val();
                if (forthapp == "" || forthapp == null) {
                    forthapp = 0;
                }



                var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(firstexxx) - parseInt(firstsurr) + parseInt(secondexx)
                    - parseInt(secondesurr) + parseInt(firstappp)
                    + parseInt(secondappp) + parseInt(thirdapp) + parseInt(forthapp);

                 $tr.find('.perposed').val(total);

                getfirstsurrenderTotal();
                 
                 getGrandTotal();
                 onloadTotalvar();

            });
            $('table').on('keyup', '.txtsecondsurrender', function () {
                    var $tr = $(this).closest('tr');
                
                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                 var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                 var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                 var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }

                   var firstexxx = $tr.find('.txtfirstexcess').val();
                if (firstexxx == "" || firstexxx == null) {
                    firstexxx = 0;
                }

                var firstsurr = $tr.find('.txtfirstsurrender').val();
                if (firstsurr == "" || firstsurr == null) {
                    firstsurr = 0;
                }

                var secondexx = $tr.find('.txtsecondexcess').val();
                if (secondexx == "" || secondexx == null) {
                    secondexx = 0;
                }

                var secondesurr = $tr.find('.txtsecondsurrender').val();
                if (secondesurr == "" || secondesurr == null) {
                    secondesurr = 0;
                }

                var firstappp = $tr.find('.txtfirstapp').val();
                if (firstappp == "" || firstappp == null) {
                    firstappp = 0;
                }
                var secondappp = $tr.find('.txtsecondapp').val();
                if (secondappp == "" || secondappp == null) {
                    secondappp = 0;
                }
                var thirdapp = $tr.find('.txtthirdapp').val();
                if (thirdapp == "" || thirdapp == null) {
                    thirdapp = 0;
                }
                var forthapp = $tr.find('.txtfourthapp').val();
                if (forthapp == "" || forthapp == null) {
                    forthapp = 0;
                }



                var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(firstexxx) - parseInt(firstsurr) + parseInt(secondexx)
                    - parseInt(secondesurr) + parseInt(firstappp)
                    + parseInt(secondappp) + parseInt(thirdapp) + parseInt(forthapp);

                 $tr.find('.perposed').val(total);

                getsecondsurrenTotal();
                 
                 getGrandTotal();
                 onloadTotalvar();

            });
        });

          

       

        function getBudget(br_ID) {
            $.ajax({
                url: "GrantRelease.aspx/bindGridJquery",
                data: JSON.stringify({ brID:br_ID }),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    var html = '';

                    $.each(data.d, function (index, item) {
                        html += '<tr><input type="hidden" class="clAccount" accountId="' + item.Account + '"/>';
                        //html += '<td>' + (index + 1) + '</td>';
                        html += '<td class="Desc" >' + item.GlDesc + '</td>';
                        
                        html += '<td><input type="number" class="form-control form-control-sm text-right grant" id="txtGrant" style="width:120px" disabled="true" value="' + item.Grant + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtQuarter1" style="width:120px" id="txtQuarter1" value="'  + item.GrantinQ1 + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtQuarter2" style="width:120px" id="txtQuarter2" value="'  + item.GrantinQ2 + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtQuarter3" style="width:120px" id="txtQuarter3" value="'  + item.GrantinQ3 + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtQuarter4" style="width:120px" id="txtQuarter4" value="'  + item.GrantinQ4 + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtfirstexcess" style="width:120px" id="txtfirstexcess" value="'  + item.firstExcess + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtfirstsurrender" style="width:120px" id="txtfirstsurrender" value="'  + item.firstSurrender + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtsecondexcess" style="width:120px" id="txtsecondexcess" value="'  + item.secondExcess + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtsecondsurrender" style="width:120px" id="txtsecondsurrender" value="'  + item.secondSurrender + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtfirstapp" style="width:120px" id="txtfirstapp" value="'  + item.firstapprop + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtsecondapp" style="width:120px" id="txtsecondapp" value="'  + item.secondapprop + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtthirdapp" style="width:120px" id="txtthirdapp" value="'  + item.thirdapprop + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtfourthapp" style="width:120px" id="txtfourthapp" value="'  + item.forthapprop + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right perposed" disabled="true" style="width:120px" id="txtPerposed" value="' + item.Aid + '"/></td>';
                        html += '</tr>';
           
                    });
                     
                    $('#myBody').html(html);
                    onloadTotalGrants();
                    onloadTotalvar();
                    getApprovedTotal();
                    getQ1Total();
                    getQ2Total();
                    getQ3Total();
                    getQ4Total();
                    getfirstexcessTotal();
                    getfirstsurrenderTotal();
                    getsecondexcessTotal();
                    getsecondsurrenTotal();
                    getfirstappTotal();
                    getsecondappTotal();
                    getthirdappTotal();
                    getforthappTotal();
                    getGrandTotal();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        }



                  

        function getApprovedTotal() {

                var val1 = 0;
                        $("#myBody [id^=txtGrant]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val1 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".totalApprovedBudget").val(val1);
                        
        }

        function getQ1Total() {

                var val2 = 0;
                        $("#myBody [id^=txtQuarter1]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val2 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".totalQ1").val(val2);
                        
        }
             function getQ2Total() {

                var val3 = 0;
                        $("#myBody [id^=txtQuarter2]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val3 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".totalQ2").val(val3);
                        
        }
             function getQ3Total() {

                var val4 = 0;
                        $("#myBody [id^=txtQuarter3]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val4 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".totalQ3").val(val4);
                        
        }

        function getQ4Total() {

                var val5 = 0;
                        $("#myBody [id^=txtQuarter4]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val5 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".totalQ4").val(val5);
                        
        }

         function getfirstexcessTotal() {

                var val6 = 0;
                        $("#myBody [id^=txtfirstexcess]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val6 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".firstexcess").val(val6);
                        
        }
        function getfirstsurrenderTotal() {

                var val14 = 0;
                        $("#myBody [id^=txtfirstsurrender]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val14 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".firstSurrender").val(val14);
                        
        }

        function getsecondexcessTotal() {

                var val7 = 0;
                        $("#myBody [id^=txtsecondexcess]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val7 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".secondexcess").val(val7);
                        
        }
        function getsecondsurrenTotal() {

                var val17 = 0;
                        $("#myBody [id^=txtsecondsurrender]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val17 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".secondsurrender").val(val17);
                        
        }
        function getfirstappTotal() {

                var val8 = 0;
                        $("#myBody [id^=txtfirstapp]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val8 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".firstapp").val(val8);
                        
        }
        function getsecondappTotal() {

                var val9 = 0;
                        $("#myBody [id^=txtsecondapp]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val9 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".secondapp").val(val9);
                        
        }
        function getthirdappTotal() {

                var val10 = 0;
                        $("#myBody [id^=txtthirdapp]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val10 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".thirdapp").val(val10);
                        
        }

        function getforthappTotal() {

                var val11 = 0;
                        $("#myBody [id^=txtfourthapp]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        val11 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".forthapp").val(val11);
                        
        }

        function getGrandTotal() {

                var valT = 0;
                        $("#myBody [id^=txtPerposed]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                        //Add values multiple Values of a single column
                        valT += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".grandTotal").val(valT);
                        
        }
        function onloadTotalGrants() {
            $('.clAccount').each(function (index, item) {

                var accountId = $(this).attr('accountId');
                var $tr = $(this).closest('tr');

                var q1 = $tr.find('.txtQuarter1').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                var q2 = $tr.find('.txtQuarter2').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }

                var q3 = $tr.find('.txtQuarter3').val();
                if (q3 == "" || q3 == null) {
                    q3 = 0;
                }
                var q4 = $tr.find('.txtQuarter4').val();
                if (q4 == "" || q4 == null) {
                    q4 = 0;
                }
                var FirstExx = $tr.find('.txtfirstexcess').val();
                if (FirstExx == "" || FirstExx == null) {
                    FirstExx = 0;
                }

                var Firstsurr = $tr.find('.txtfirstsurrender').val();
                if (Firstsurr == "" || Firstsurr == null) {
                    Firstsurr = 0;
                }

                var Sec = $tr.find('.txtsecondexcess').val();
                if (Sec == "" || Sec == null) {
                    Sec = 0;
                }

                var Secsur = $tr.find('.txtsecondsurrender').val();
                if (Secsur == "" || Secsur == null) {
                    Secsur = 0;
                }

                var firapp = $tr.find('.txtfirstapp').val();
                if (firapp == "" || firapp == null) {
                    firapp = 0;
                }
                var secapp = $tr.find('.txtsecondapp').val();
                if (secapp == "" || secapp == null) {
                    secapp = 0;
                }
                var thiapp = $tr.find('.txtthirdapp').val();
                if (thiapp == "" || thiapp == null) {
                    thiapp = 0;
                }
                var forapp = $tr.find('.txtfourthapp').val();
                if (forapp == "" || forapp == null) {
                    forapp = 0;
                }

                var total = parseInt(q1) + parseInt(q2) + parseInt(q3) + parseInt(q4) + parseInt(FirstExx) - parseInt(Firstsurr) + parseInt(Sec)
                    - parseInt(Secsur) + parseInt(firapp)
                    + parseInt(secapp) + parseInt(thiapp) + parseInt(forapp);

                $tr.find('.perposed').val(total);
            });
        }
        function onloadTotalvar() {
                debugger
            $('.clAccount').each(function (index, item) {

                var accountId = $(this).attr('accountId');
                var $tr = $(this).closest('tr');

                var q1 = $tr.find('.grant').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                var q2 = $tr.find('.perposed').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }
                var variance = parseInt(q1) - parseInt(q2);
                $tr.find(".var").val(variance);



            });

        }
    </script>


   

    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />


    
    &nbsp;
    <br />


    <div class="card">
        <div class="card-body">
            <div class="row">
        <div class="col-sm-12">
            <uc1:Messages ID="ucMessage" runat="server" />
        </div>
           </div>

            <div class="row">
                <div class="col-md-4">
                    <label>Divisions:</label>
                    <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" 
                        AppendDataBoundItems="True">
                        <asp:ListItem Value="0">Punjab Arts Council</asp:ListItem>
                    </asp:DropDownList>

                </div>
                <div class="col-md-4">
                    <br />
                   
                    <input type="button" class="btn btn-lg btn-primary"  id="btnSave" value="Save Budget" />
                </div>
                <div class="col-lg-2 col-md-2 col-sm-2 offset-lg-2" style="margin-top:15px;">
                    <asp:Button Text="Report" CssClass="btn btn-primary click" runat="server" />
                </div>
            </div>

           


            <div class="succesmessage">

            </div>
            <div id="myGrid">
                <table id="myTable" class="table table-responsive table-sm">
                    <thead>
                        <tr class="header">
                           <%-- <th>Serial
                        </th>--%>
                        <th>Account
                       
                        </th>
                            <th>
                                Grant
                            </th>

                            <th>
                                Q1
                            </th>
                            <th>
                                Q2
                            </th>
                            <th>
                                Q3
                            </th>
                            <th>
                                Q4
                            </th>
                            <th>
                               1st Excess
                            </th>
                            <th>
                               1st Surrender
                            </th>
                            <th>
                                2nd Excess
                            </th>
                            <th>
                                2nd Surrender
                            </th>
                            <th>
                                1st Appropriation(if any)
                            </th>
                            <th>
                                2nd Appropriation(if any)
                            </th>
                            <th>
                                3rd Appropriation(if any)
                            </th>
                            <th>
                                4th Appropriation(if any)
                            </th>
                            <th>
                                Total
                            </th>
                        </tr>
                    </thead>
                    <tbody id="myBody">
                    </tbody>
                </table>
            </div>


            <div class="row">
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>Total</label>
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>Grant Total</label>
                        <input type="text" readonly="readonly" class="form-control totalApprovedBudget" name="name" value="" />
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>1st Quarter Total</label>
                        <input type="text" readonly="readonly" class="form-control totalQ1" name="name" value="" />
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>2nd Quarter Total</label>
                        <input type="text" readonly="readonly" class="form-control totalQ2" name="name" value="" />
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>3rd Quarter Total</label>
                        <input type="text" readonly="readonly" class="form-control totalQ3" name="name" value="" />
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>4th Quarter Total</label>
                        <input type="text" readonly="readonly" class="form-control totalQ4" name="name" value="" />
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>1st Excess Total</label>
                        <input type="text" readonly="readonly" class="form-control firstexcess" name="name" value="" />
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>1st Surrender Total</label>
                        <input type="text" readonly="readonly" class="form-control firstSurrender" name="name" value="" />
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>2nd Excess Total</label>
                        <input type="text" readonly="readonly" class="form-control secondexcess" name="name" value="" />
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>2nd Surrender Total</label>
                        <input type="text" readonly="readonly" class="form-control secondsurrender" name="name" value="" />
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>1st Appropriation Total</label>
                        <input type="text" readonly="readonly" class="form-control firstapp" name="name" value="" />
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>2nd Appropriation Total</label>
                        <input type="text" readonly="readonly" class="form-control secondapp" name="name" value="" />
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>3rd Appropriation Total</label>
                        <input type="text" readonly="readonly" class="form-control thirdapp" name="name" value="" />
                    </div>
                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>4th Appropriation Total</label>
                        <input type="text" readonly="readonly" class="form-control forthapp" name="name" value="" />
                    </div>
                </div>

            </div>


            <%--<div class="row">
                <div class="col-md-4">
                    <div class="row">
                         <div class="col-md-6">
                             <label><b>Total</b></label>
                         </div>
                         <div class="col-md-6">
                             <div class="row">
                                 <div class="col-md-6">
                                     <input  type="text" readonly="readonly" class="form-control totalApprovedBudget"/>
                                 </div>
                                 <div class="col-md-6">
                                     <input  type="text" readonly="readonly" class="form-control totalQ1"/>
                                 </div>
                             </div>
                         </div>
                    </div>
                </div>
                 <div class="col-md-8">

                    <div class="row">
                          <div class="col-md-6">
                             <div class="row">
                                 <div class="col-md-6">
                                     <input  type="text" readonly="readonly" class="form-control totalQ2"/>
                                 </div>
                                 <div class="col-md-6">
                                     <input  type="text" readonly="readonly" class="form-control totalQ3"/>

                                 </div>
                             </div>
                         </div>
                        
                         <div class="col-md-6">
                             <div class="row">
                                 <div class="col-md-6">
                                     <input  type="text" readonly="readonly" class="form-control totalQ4"/>
                                 </div>
                                 <div class="col-md-6">
                                     <input  type="text" readonly="readonly" class="form-control grandTotal"/>

                                 </div>
                             </div>
                         </div>
                    </div>
                   
                </div>
            </div>--%>

        </div>
    </div>













<%--
    <div class="row">
        <div class="col-sm-12">
            <uc1:Messages ID="ucMessage" runat="server" />
        </div>
    </div>

    <div class="row">
        <div class="col-sm-12">
            <asp:GridView ID="grdBudget" CssClass="table table-responsive-sm table-bordered font-weight-light bg-white" runat="server" DataKeyNames="Account" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="txtcode" Text='<%#Eval("Account") %>' Width="120px" CssClass="form-control form-control-sm"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Account">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="txtDsc" Text='<%#Eval("GlDesc") %>' CssClass="form-control form-control-sm"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Income">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtIncome" Text='<%#Eval("Income") %>' Width="100px" CssClass="form-control form-control-sm income" Style="text-align: right" TextMode="Number"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Grant">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtGrant" Text='<%#Eval("Grant") %>' Width="100px" CssClass="form-control form-control-sm grant" Style="text-align: right" TextMode="Number"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Approved">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtAid" Text='<%#Eval("Aid") %>' Width="100px" CssClass="form-control form-control-sm" Style="text-align: right" TextMode="Number"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <asp:Button runat="server" ID="btnSave" class="btn btn-primary" Text="Save" OnClick="BtnSave_Click"></asp:Button>
        </div>
    </div>--%>
</asp:Content>
