const n=10;

var a:array [0..n-1] of pinteger;
    p:pinteger;
    s:string;
begin
  //a[0]:=nil;
  new(a[0]);
  //a[0]^:=0;
  {for var i:=0 to n-1 do begin
    write(i);
    new(a[i]);
    a[i]^:=i;
  end;
  while true do begin
    s:='';
    for var i:=0 to n-1 do begin
      s:=S+a[i]^.tostring;
      writeln(a[i]^);
    end;
  end;}
    

end.