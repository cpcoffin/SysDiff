param([string]$infile, [string]$outfile)

$lines = gc $infile | ? { $_ -notlike 'dummy*' }
$out = @()
foreach ($i in $lines)
{
 foreach ($j in $lines)
 {
  $out += "$i,$j"
 }
}

$out | out-file $outfile -encoding ascii