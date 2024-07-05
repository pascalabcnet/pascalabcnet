uses u_events6, System;

type TClass = class(I1)
  event E1: Action;
  
  procedure RaiseMethod;
  begin
    if E1 <> nil then
      E1();
  end;

end;

begin
  var i := 0;
  var o := new TClass;
  o.E1 += () -> begin Inc(i) end;
  o.RaiseMethod();
  assert(i = 1);
end.