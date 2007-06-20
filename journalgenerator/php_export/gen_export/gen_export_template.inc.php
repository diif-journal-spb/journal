<?


function parse($text) {
 $text = str_replace( "<br>", "<br />", $text);
 $text = str_replace( "&", "&amp;", $text);
 $text = str_replace( "<", "&lt;", $text);
 $text = str_replace( ">", "&gt;", $text);

 return $text;

}


function createRFFIXML($number, $year) {

    $an = getArticleByNumberYear($number, $year);

    $ar = $an->getNext();

    $journalPageMin = $ar->pageMin;

    while (($an->hasNext()) == 1) {
        $ar = $an->getNext();
    } 

    $journalPageMax = $ar->pageMax;


   $titleRu = "Дифференциальные Уравнения и Процессы Управления";
   $titleEn = "Electronic Journal Difference Equations and Control Processes";

   $journalCode = "123456789";

   $journalYear = $year;
   $journalDateUni = $year.($number * 3 - 2)."/".( $number * 3 );
   $journalNum = $number;


//header

print "<?xml version=\"1.0\" encoding=\"koi8-r\" standalone=\"no\" ?>";
?>
  <number year="<?=$year?>" number="<?=$number?>">
<?    
   printArticles($year, $number);
?>
  </number>
<?
}

function printArticles($year, $number) {

    $an = getArticleByNumberYear($number, $year);


    while (($an->hasNext()) == 1) {
        $ar = $an->getNext();
        printArticle( $ar, $year, $number );
    } 
}


function printArticle($article, $year, $number) {    
  $pageMin = $article->pageMin;
  $pageMax = $article->pageMax;
           
  $filePdf = $article->en->pdfLink;

  $titleEn = $article->en->Name;
  $titleRu = $article->ru->Name;

  $textEn = $article->en->abstract;
  $textRu = $article->ru->abstract;

  $abstractEn = $article->en->abstract;
  $abstractRu = $article->ru->abstract;



?>           
          <article>
            <articleInfo lang="EN" FirstPage="<?=$pageMin?>" LastPage="<?=$pageMax?>">
              <pdf><?=parsePath($filePdf)?></pdf>
              <abstract><?=parse($abstractEn)?></abstract>
              <title><?=parse($titleEn)?></title>
            </articleInfo>
            <articleInfo lang="RU" FirstPage="<?=$pageMin?>" LastPage="<?=$pageMax?>">
              <pdf><?=parsePath($filePdf)?></pdf>
              <abstract><?=parse($abstractRu)?></abstract>
              <title><?=parse($titleRu)?></title>
            </articleInfo>
            <authors>
                <? printAuthorIds($article, $year, $number); ?>
            </authors>
          </article>
          
          <authors>
             <? printAuthors($article, $year, $number); ?>
          </authors>               


<?
}


function parsePath($path) {

 $arr = split('/',$path);
 $ind = count($arr);

 return $arr[$ind-1];
}

function authorId($article, $year, $number, $i)
{
  return "export_n".$number."_y".$year."_a".$article->id."_i".$i;
}


function printAuthorIds($atricle, $year, $number) {
  global $authNum;
  $ru = $atricle->ru;
  $en = $atricle->en;

  for ($i = 0; $i<$ru->Authors->cnt; $i++) {
     ?><author ref="<?=authorId($atricle, $year, $number, $i)?>" /><?
  }
}


function printAuthors($atricle, $year, $number) {
  global $authNum;
  $ru = $atricle->ru;
  $en = $atricle->en;

  for ($i = 0; $i<$ru->Authors->cnt; $i++) {
?>
    <author id="<?=authorId($atricle, $year, $number, $i)?>">
      <author lang="EN">
        <FirstName><?=parse($en->Authors->firstName[$i])?></FirstName>
        <MiddleName><?=parse($en->Authors->middleName[$i])?></MiddleName>
        <LastName><?=parse($en->Authors->lastName[$i])?></LastName>
        <Email><?=parse($en->Authors->email[$i])?></Email>
        <Address><?=parse($en->Authors->Address[$i])?></Address>
      </author>
      <author lang="RU">
        <FirstName><?=parse($ru->Authors->firstName[$i])?></FirstName>
        <MiddleName><?=parse($ru->Authors->middleName[$i])?></MiddleName>
        <LastName><?=parse($ru->Authors->lastName[$i])?></LastName>
        <Email><?=parse($ru->Authors->email[$i])?></Email>
        <Address><?=parse($ru->Authors->Address[$i])?></Address>
      </author>
    </author>
<?
  }
}
?>