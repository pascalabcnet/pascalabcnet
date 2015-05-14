var s: string;
  
begin
  s := 'ABCDEFGH';
  writeln(LowerCase(s));
  writeln(ReverseString(s));
  writeln(RightStr(s,5));
  writeln(StringOfChar('a',10));
  
  s := IntToStr(12345);
  var i: integer := StrToInt(s);
end.