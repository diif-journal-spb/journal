<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
   "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="ru" lang="ru">
  <head>
    <title>Список статей. $Number$-$Year$</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"></meta>
    <style type="text/css">
       body {
			  background-color: rgb(240, 248, 255);
			  font-family: Tahoma, Verdana, Helvetica, Arial;
			  color: rgb(100, 60, 40);
			  font-size: 12pt;
			  margin:0;
			  padding:0;
			  padding-bottom: 5em;
       }
       
       h1 {
			  font-family: Impact, Helvetica, Arial, Verdana;
			  text-align: center;
			  font-size: 22pt;
			  font-weight: bold;
			  color: rgb(120, 64, 0);
			  padding:0;
			  margin:0;
			}

		h2 {
		  font-size: 14pt;
		  font-weight: bold;
		  padding-left: 20px;
		}
    </style>
  </head>
  
  <body>
  <h1>Электронный журнал "Дифференциальные Уравнения и Процессы Управления"</h1>
  <h2>Номер $Number$ $Year$</h2>
  <ul>  
  $Articles:{    
    <li>$it.Authors$ <a href="$it.Url$">$it.Name$</a></li>
  }$
  </ul>
  
  </body>
</html>