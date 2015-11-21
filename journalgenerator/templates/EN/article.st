<h2>$article.Title$</h2>
<h3>Author(s):</h3>
$article.Authors:{
  <p><b>$it.FirstName$ $it.MiddleName$ $it.LastName$</b></p>
  <p>$it.Address$</p>
  <p><a href="mailto:$it.EMail$">$it.EMail$</a></p>
}$

<h3>Abstract:</h3>
<p>$article.Abstract$</p>

$if(article.HasKeywords)$
<a name="keywords"></a>
<h3 name="keywords">Keywords</h3>

<ul>
$article.Keywords:{
  <li class="paper_keyword">$it$</li>
}$
</ul>
$endif$

$if(article.HasReferences)$
<a name="refs"></a>
<h3>References:</h3>

<ol>
$article.References:{
  <li class="paper_reference">$it.Title$</li>
}$
</ol>
$endif$

<h3><a href="$PdfLink$">Full text (pdf)</a></h3>
