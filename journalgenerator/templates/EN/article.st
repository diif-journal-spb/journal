<h2>$article.Title$</h2>
<h3>Author(s):</h3>
$article.Authors:{
  <p><b>$it.FirstName$ $it.MiddleName$ $it.LastName$</b></p>
  <p>$it.Address$</p>
  <p><a href="mailto:$it.EMail$">$it.EMail$</a></p>
}$

<h3>Abstract:</h3>
<p>$article.Abstract$</p>

$if(article.HasReferences)$
<h3>References:</h3>
<a href="refs"></a>
<ol>
$article.References:{
  <li>$it.Title$</li>
}$
</ol>
$endif$

<h3><a href="$PdfLink$">Full text (pdf)</a></h3>
