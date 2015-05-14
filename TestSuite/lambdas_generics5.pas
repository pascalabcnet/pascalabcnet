begin
  var a := new System.Collections.Generic.List<string>();
  a.Add('abc');
  a.Add('def');
  var b := a.SelectMany(x -> x).Select(x -> x);
  foreach var c in b do
    writeln(c);
end.