<h2>
  Номер #$issue.Number$ за $issue.Year$
</h2>


$sections:{
  $if(it.ShowTitle)$
  <h2>$it.TitleName$</h2>
  $endif$
  
  $it.articles:{ 
  <div class="article">
    <h3><a href="$it.Link$">$it.Info.Title$</a></h3>
    $it.Info.Authors:{<h5>$it.FirstName$ $it.MiddleName$ $it.LastName$</h5>}$
    <p>
      <a href="$it.Pdf$">Полный текст (pdf)</a> 
      |
      <a href="$it.Link$">Аннотация</a>
      $if(it.HasReferences)$
      |
      <a href="$it.Link$#refs">Ссылки</a>
      $endif$
    </p>
   </div>
  }$
}$



