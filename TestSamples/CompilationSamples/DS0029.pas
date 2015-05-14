const n=10;

var a:array [0..n-1] of pinteger;
    p:pinteger;
    s:string;
begin
  for var i:=0 to n-1 do begin
    new(p);
    a[i]:=p;
    a[i]^:=i;
  end;
  while true do begin
    //s:='s';
    for var i:=0 to n-1 do begin
      writeln('a[',i,']=',a[i]^);
      s:=a[i]^.tostring;//нет боксировки
    end;
    writeln;
  end;
end.