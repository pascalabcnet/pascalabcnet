type TClass = class(List<integer>)
  procedure test;
  begin
    Add(2);
    self[0] += 2;
  end;
end;
begin
  var o := new TClass;
  o.test;
  assert(o[0] = 4);
end.