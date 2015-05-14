unit u_stringelem2;
procedure Test(var c: char);
begin
  c := 'k';  
end;

var
  s: string := 'abcd';
  s2: string[5] := '1234';

begin
  Test(s[2]);
  Test(s2[2]);
  assert(s = 'akcd');
  assert(s2 = '1k34');
end.