unit DS0018_unit;

type cl=class 
  procedure q;
end;

var f:cl;

procedure p(ccc:cl);
begin
  
end;


procedure cl.q;
var b:boolean;
begin
  b:=f=nil;
  writeln(b?'ERROR':'OK');
  p(f);
end;


end.