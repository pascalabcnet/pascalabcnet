function SortString(s: string): string;
begin
  var sb := new StringBuilder(s.Length, s.Length);
  sb.Append(s.ToSortedSet.ToArray);
  Result := sb.ToString;
end;

begin
  var sa := Arr('881237');
  for var i := 0 to sa.Length - 1 do
    sa[i] := SortString(sa[i]);
  var s: string;
  var c: Dictionary<char, integer>;
  while true do
  begin
    s := '';
    while (string.IsNullOrWhiteSpace(s)) do
      s := ReadLexem.ToLower;
    c := SortString(s).EachCount;
    foreach var s2 in sa do
      if ((s2.EachCount.Keys = c.Keys) and (s2.EachCount.Values = c.Values)) then
        Writeln(s2)
      else
      begin
        Writeln(s2.EachCount);
        Writeln(c);
      end;
    Writeln;
  end;
end.