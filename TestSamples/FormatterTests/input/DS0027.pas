const n=1000;

var a:array of pinteger;
    p:pinteger;
    s:string;
begin
  SetLength(a,n);
  for var i:=0 to n-1 do begin
    new(a[i]);
    a[i]^:=i;
  end;
  while true do begin
    s:='';
    for var i:=0 to n-1 do begin
      s:=S+a[i]^.tostring;
      writeln(a[i]^);
    end;
  end;
    

end.