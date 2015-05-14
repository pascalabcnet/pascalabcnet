const n=100;

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
    s:='';
    for var i:=0 to n-1 do begin
      writeln('a[',i,']=',a[i]^);
      var v:=a[i]^;
      s:=s+v.tostring;
    end;
    writeln;
  end;
end.