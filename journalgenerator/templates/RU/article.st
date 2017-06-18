<h2>$article.Title$</h2>
$if(article.HasAbstracts)$
<h3>Автор(ы):</h3>
$article.Authors:{
  <p><b>$it.FirstName$ $it.MiddleName$ $it.LastName$</b></p>
  <p>$it.Address$</p>
  <p><a href="mailto:$it.EMail$">$it.EMail$</a></p>
}$
$endif$

$if(article.HasAbstracts)$
<h3>Аннотация:</h3>
<p>$article.Abstract$</p>
$endif$

$if(article.HasKeywords)$
<a name="keywords"></a>
<h3>Ключевые слова</h3>

<ul>
$article.Keywords:{
  <li class="paper_keyword">$it$</li>
}$
</ul>
$endif$

$if(article.HasReferences)$
<a name="refs"></a>
<h3>Ссылки:</h3>

<ol>
$article.References:{
  <li class="paper_reference">$it.Title$</li>
}$
</ol>
$endif$

<h3><a href="$PdfLink$">Полный текст (pdf)</a></h3>
