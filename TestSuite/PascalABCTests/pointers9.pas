type
  r1 = record end;
  
var h1: ^r1;
    hh1: ^^r1;
    
procedure p1(var h: ^r1);
begin
  var hh:=@h;
  assert(hh = hh1);
  assert(h = h1);
end;
 
begin
  New(h1);
  hh1 := @h1;
  p1(h1);
end.