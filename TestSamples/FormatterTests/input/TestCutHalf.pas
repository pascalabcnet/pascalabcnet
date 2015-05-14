//uses Halfs;
uses Half;


procedure CutHalf(var s: string);
begin
  Assert(s.Length <> 0);
  SetLength(s, s.Length div 2);
end;


begin
  
  var sm := 'aaabbb';
  CutHalf(sm);
  writeln(sm);
  
  var sh := 'cccddd';
  Half.CutHalf(sh);
  writeln(sh);
  
  var shs := 'eeefff';
  //Half.CutHalf(shs);
  writeln(shs);
  
end.