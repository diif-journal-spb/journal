<h2>
  Issue $issue.Year$ #$issue.Number$
</h2>

$articles:{ 
  <h3>
    <a href="$it.Link$">
      $it.Info.Title$
    </a>
  </h3>
  <div class="article">

    $it.Info.Authors:{ 
      <h5><b>$it.FirstName$ $it.MiddleName$ $it.LastName$</b></h5> 
    }$

    <p>
      <a href="$it.Pdf$">Full text (pdf)</a> 
      |
      <a href="$it.Link$">Abstract</a>
    </p>
 </div>
}$