<h2>
  ����� #$issue.Number$ �� $issue.Year$
</h2>

$articles:{  
  <div class="article">
    <h3><a href="$it.Link$">$it.Info.Title$</a></h3>
    $it.Info.Authors:{<h5>$it.FirstName$ $it.MiddleName$ $it.LastName$</h5>}$
    <p>
      <a href="$it.Pdf$">������ ����� (pdf)</a> 
      |
      <a href="$it.Link$">�������</a>
    </p>
 </div>
}$