<h2>
  Номер #$issue.Number$ за $issue.Year$
</h2>

$articles:{  
  <div class="article">
    <h3><a href="$it.Link$">$it.Info.Title$</a></h3>
    $it.Info.Authors:{<h5>$it.FirstName$ $it.MiddleName$ $it.LastName$</h5>}$
    <p>
      <a href="$it.Pdf$">Полный текст (pdf)</a> 
      |
      <a href="$it.Link$">Реферат</a>
    </p>
 </div>
}$
