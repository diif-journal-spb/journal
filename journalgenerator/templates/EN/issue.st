<h2>
  Issue $issue.Year$ #$issue.Number$
</h2>

$sections:{
  $if(it.ShowTitle)$
  <h2>$it.TitleName$</h2>
  $endif$
  
  $it.articles:{ 
  <div class="article">
    <h3><a href="$it.Link$">$it.Info.Title$</a></h3>
    $it.Info.Authors:{ <h5>$it.FirstName$ $it.MiddleName$ $it.LastName$</h5>  }$
    <p>
      <a href="$it.Pdf$">Full text (pdf)</a> 
      $if(it.HasAbstracts)$
      |
      <a href="$it.Link$">Abstract</a>
      $endif$
      $if(it.HasReferences)$
      |
      <a href="$it.Link$#refs">References</a>
      $endif$
      $if(it.HasKeywords)$
      |
      <a href="$it.Link$#keywords">Keywords</a>
      $endif$   
    </p>
 </div>
  }$
}$

