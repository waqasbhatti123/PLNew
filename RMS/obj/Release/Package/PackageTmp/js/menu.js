function mainmenu()
{
try
{
$("#nav ul").css({display: "none"}); // Opera Fix
$(" #nav li").hover(function(){
		$(this).find('ul:first').css({visibility: "visible",display: "none"}).show(400);
		},function(){
		$(this).find('ul:first').css({visibility: "hidden"});
		});

}
catch(Error)
{
return true;
}
 
 
 $(document).ready(function(){					
	mainmenu();
})};