<h2>
  Issue $issue.Year$ #$issue.Number$
</h2>

$articles:{ 
  <div class="article">
    <h3><a href="$it.Link$">$it.Info.Title$</a></h3>
    $it.Info.Authors:{ <h5>$it.FirstName$ $it.MiddleName$ $it.LastName$</h5>  }$
    <p>
      <a href="$it.Pdf$">Full text (pdf)</a> 
      |
      <a href="$it.Link$">Abstract</a>
    </p>
 </div>
}$