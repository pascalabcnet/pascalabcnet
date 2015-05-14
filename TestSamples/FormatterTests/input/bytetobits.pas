function ByteToBits(b:byte):array of byte;
var e: byte;
begin
  e := 1;
  SetLength(result,8);
  for var i:=0 to 7 do begin
    result[i] := e and b > 0 ? 1 : 0;
    e := e shl 1;
  end;
end;

function BitsToByte(b:array of byte):byte;
var e,r: byte;
begin
  e := 1;
  r := 1;
  for var i:=0 to 7 do begin
    result := result + b[i]*r;
    r := r*2;
    e := e shl 1;
  end;
end;

var res: array of byte;
begin
  res := ByteToBits(100);
  foreach b:byte in res do
    write(b,' ');
  writeln;
  write(BitsToByte(res));
end.