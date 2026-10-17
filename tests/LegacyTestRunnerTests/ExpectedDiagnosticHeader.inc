// Diagnostic annotations occupy the first physical line, independent of OS line endings.
function ExpectedDiagnosticMessage(content, commentSymbol: string): string;
begin
  var prefix := commentSymbol + '!';
  if not content.StartsWith(prefix) then
    exit('');
  var lineEnd := content.IndexOfAny([#13, #10]);
  if lineEnd < 0 then
    lineEnd := content.Length;
  Result := content.Substring(prefix.Length, lineEnd - prefix.Length).Trim;
end;
