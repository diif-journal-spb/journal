<h2>
  Issue $issue.Year$ #$issue.Number$
</h2>

$articles:{ 
  <h3>
    <a href="$it.Link$">
      $it.Info.Title$
    </a>
  </h3>
  $it.Info.Authors:{ 
  <p>
    $it.FirstName$ $it.MiddleName$ $it.LastName$
  </p>
  }$
  <p>
    <a href="$it.Pdf$">
      Full text (pdf)
    </a>
  </p>
}$