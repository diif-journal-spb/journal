<h2>$article.Title$</h2>
<h3>Author(s):</h3>
$article.Authors:{
  <p>$it.FirstName$ $it.MiddleName$ $it.LastName$</p>
  <p>$it.Address$</p>
  <p><a href="mailto:$it.EMail$">$it.EMail$</a></p>
}$

<h3>Abstract:</h3>
<p>$article.Abstract$</p>

<h3><a href="$PdfLink$">Полный текст (pdf)</a></h3>