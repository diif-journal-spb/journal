<h2>
  Books
</h2>
$books:{

<h3>
  $it.Title$
</h3>
<h3>$it.Authors:{$it,separator=","}$</h3>
<img src="$it.Image$" alt="title"/>
<p>
  $it.Abstract$
</p>
<p>$it.Date$</p>

}$