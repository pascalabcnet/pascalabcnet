var
  a, b: string;

begin
  a := '1';
  b := '1';
  WriteAllText('input.txt','45');
  assign(input,'input.txt');
  readln(a[1], b[1]);
  assert(a[1] = '4');
  assert(b[1] = '5');
end.