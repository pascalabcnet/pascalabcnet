var s:='ф';
begin
  writeln(Ord(s));
  writeln(OrdUnicode(s));
  writeln(Chr(Ord(s)));
  writeln(ChrUnicode(OrdUnicode(s)));
  writeln(integer(s));
end.