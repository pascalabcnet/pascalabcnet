procedure p(var Self: string); extensionmethod;
begin
  self[1] := self[1].ToUpper;
end;

begin
  var s := 'asd';
  s.p;
  Assert(s[1]='A');
end.