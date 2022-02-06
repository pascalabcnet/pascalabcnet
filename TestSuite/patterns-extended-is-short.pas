type
  t1=class end;
  t2=class(t1) end;

begin
  var l: List<t1> := nil;
  if (l <> nil) and (l[0] is t2(var a)) then ;
  l := new List<t1>();
  var o := new t2;
  l.Add(o);
  if (l <> nil) and (l[0] is t2(var a)) then
  begin
    assert(a = o);
    o := nil;
  end;
  assert(o = nil);
  l := new List<t1>();
  l.Add(new t1);
  if (l <> nil) and (l[0] is t2(var a)) then
  begin
    o := new t2;
  end;
  assert(o = nil);
end.