<h2>
  $article.Title$
</h2>
<h3>
  Автор(ы):
</h3>

$article.Authors:{
  <p>
    $it.FirstName$ $it.MiddleName$ $it.LastName$
  </p>
  <p>
    $it.Address$
  </p>
  <p>
    <a href="mailto:$it.EMail$">
      $it.EMail$
    </a>
  </p>
}$

<h3>
  Реферат:
</h3>
<p>
  $article.Abstract$
</p>
<h3>
  Полный текст:
</h3>
<a href="$PdfLink$">
  pdf
</a>
