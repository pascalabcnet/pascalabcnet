[CmdletBinding()]
param([string]$HighlightingDirectory)
$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $root 'bin\ICSharpCode.TextEditor.dll'))
$manager = [ICSharpCode.TextEditor.Document.HighlightingManager]::Manager
if (-not $HighlightingDirectory) { $HighlightingDirectory = Join-Path $root 'bin\Highlighting' }
$provider = [ICSharpCode.TextEditor.Document.FileSyntaxModeProvider]::new($HighlightingDirectory)
$manager.AddSyntaxModeFileProvider($provider)
$strategy = $manager.FindHighlighterForFile('test.pas')

function Check-Color($document, [int]$line, [string]$word, [string]$color) {
    $segment = $document.GetLineSegment($line)
    $column = $document.GetText($segment.Offset, $segment.Length).IndexOf($word, [StringComparison]::Ordinal)
    if ($column -lt 0) { throw "Missing test word: $word" }
    $actual = $segment.GetColorForPosition($column).Color
    if ($actual.ToArgb() -ne [Drawing.Color]::FromName($color).ToArgb()) {
        throw "Expected $color for '$word' on line $($line + 1), got $actual"
    }
}

foreach ($newline in @([string][char]10, [string][char]13 + [char]10)) {
    $doc = [ICSharpCode.TextEditor.Document.DocumentFactory]::new().CreateDocument()
    $doc.HighlightingStrategy = $strategy
    $q = [string][char]39
    $triple = $q * 3
    $doc.TextContent = @(
        'begin',
        ('  var s := $' + $triple),
        "    It's text {age + 1}",
        ('    {{text}} {' + $q + 'quoted' + $q + '}'),
        ('    ' + $triple + ';'),
        '  Println(s);',
        ('  var plain := ' + $triple),
        '    {plain text}',
        ('    ' + $triple + ';'),
        ('  var inline := $' + $q + 'age: {age}' + $q + ';'),
        'end.'
    ) -join $newline
    $strategy.MarkTokens($doc)
    Check-Color $doc 2 "It's" Blue
    Check-Color $doc 2 'age' Black
    Check-Color $doc 3 'text' Blue
    Check-Color $doc 3 'quoted' Blue
    Check-Color $doc 5 'Println' Black
    Check-Color $doc 7 'plain' Blue
    Check-Color $doc 9 'age:' Blue
    Check-Color $doc 10 'end' Black
    if (-not $doc.GetLineSegment(10).GetColorForPosition(0).Bold) { throw 'end is not a keyword' }

    # Exercise incremental highlighting when the user adds/removes the prefix.
    $prefixOffset = $doc.TextContent.IndexOf('$', [StringComparison]::Ordinal)
    $doc.Remove($prefixOffset, 1)
    Check-Color $doc 2 'age' Blue
    $doc.Insert($prefixOffset, '$')
    Check-Color $doc 2 'age' Black
    Check-Color $doc 5 'Println' Black
    Check-Color $doc 10 'end' Black
}
'PASS multiline and single-line interpolation highlighting, LF and CRLF'
