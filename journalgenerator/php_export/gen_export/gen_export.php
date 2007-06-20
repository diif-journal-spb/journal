<?

  
  // header("Content-Type: plain/text");


   require_once('../articles.inc.php');
   require_once('../inc/articleClass.inc.php');
   require_once('../inc/utils.php');

   require_once('gen_export_template.inc.php'); 

   $year = param("year", 4);
   $number = param("num", 1);

   if (! isset($year) || $year == "") {

?>

   <form action="gen_export.php" method="POST">
	Year: <input type="text" name="year"> <br>
	Number: <input type="text" name="num"> </br>
	<input type="submit">
    </form>
<?
  } else {

   createRFFIXML($number, $year);
 
  } 

?>