// #2271
procedure p;
label 1;
begin
  Arr(3).Select(b -> b);
  if True then
    goto 1;
  1: Assert(1=1);
end;
  
begin  
  p;
end.
