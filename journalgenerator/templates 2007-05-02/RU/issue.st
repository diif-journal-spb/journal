<h2>
  Номер $issue.Year$ #$issue.Number$
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
      Полный текст (pdf)
    </a>
  </p>
}$