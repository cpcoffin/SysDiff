param ($singlekeyfile, $outfile, [int]$numkeys)


$table = import-csv $singlekeyfile
$keys = $table.key
write-host "count: $($keys.count)"
foreach ($i in 1..$numkeys)
{
    $colname = "key$i"
    $newtable = @()
    foreach ($row in $table)
    {
        foreach ($v in $keys)
        {
            $add = $true
            foreach ($x in 0..($i-1))
            {
                if ($row."key$x" -eq $v)
                {
                    $add = $false
                    break
                }
            }
            if ($add)
            {
                $newtable += $row.PsObject.Copy() | add-member -notepropertyname $colname -notepropertyvalue $v -passthru -force
            }
        }
    }
    $table = $newtable
}

$table | export-csv $outfile -notypeinformation