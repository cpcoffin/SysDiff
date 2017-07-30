param ($singlevaluefile, $outfile, [int]$numvalues)


$table = import-csv $singlevaluefile
$values = $table.value
write-host "count: $($values.count)"
foreach ($i in 0..$numvalues1)
{
    $colname = "value$i"
    $newtable = @()
    foreach ($row in $table)
    {
        foreach ($v in $values)
        {
            $add = $true
            foreach ($x in 0..($i-1))
            {
                if ($row."value$x" -eq $v)
                {
                    $add = $false
                    break
                }
            }
            if ($add)
            {
                $newtable += $row.PsObject.Copy() | select dummy,type,key | add-member -notepropertyname $colname -notepropertyvalue $v -passthru -force
            }
        }
    }
    $table = $newtable
}

$table | export-csv $outfile -notypeinformation