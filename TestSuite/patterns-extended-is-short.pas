type
  t1=class end;
  t2=class(t1) end;

begin
  var l: List<t1> := nil;
  if (l <> nil) and (l[0] is t2(var a)) then ;
end.