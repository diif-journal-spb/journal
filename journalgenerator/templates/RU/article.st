<h2>$article.Title$</h2>
<h3>Автор(ы):</h3>
$article.Authors:{
  <p><b>$it.FirstName$ $it.MiddleName$ $it.LastName$</b></p>
  <p>$it.Address$</p>
  <p><a href="mailto:$it.EMail$">$it.EMail$</a></p>
}$

<h3>Реферат:</h3>
<p>$article.Abstract$</p>

$if(article.HasReferences)$
<a name="refs"></a>
<h3>Ссылки:</h3>

<ol>
$article.References:{
  <li>$it.Title$</li>
}$
</ol>
$endif$

<h3><a href="$PdfLink$">Полный текст (pdf)</a></h3>
