type
  TBaseClass = class
    auto property p1: byte;
  end;
  TClass = class(TBaseClass)
  end;

begin
  var o: TBaseClass := new TClass;
  var o1: TBaseClass := new TBaseClass;
  o.p1 := 2;
  o1.p1 := 3;
  assert(o.p1 = 2);
  assert(o1.p1 = 3);
end.
