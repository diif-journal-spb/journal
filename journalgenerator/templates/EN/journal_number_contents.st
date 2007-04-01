<h1>Number $number.Year$ #$number.Number$ Contents</h1>


$articles:{ 
   <h1><a href="$it.Link$">$it.Info.Title$</a></h1>
   $it.Info.Authors:{ 
      <h2>$it.FirstName$ $it.MiddleName$ $it.LastName$</h2>
   }$

   <a href="$it.Pdf$">pdf</a>
 
   <hr>
}$