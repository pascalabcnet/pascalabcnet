type
  TBaseClass = class
    auto property p1: byte;
    auto property p2: integer;
    auto property p3: sequence of integer;
  end;
  
  TClass = class(TBaseClass)
  end;

begin
  var o: TBaseClass := new TClass;
  o.p1 := 2;
  o.p2 := 398;
  o.p3 := Seq(1,2,3);
  assert(o.p1 = 2);
  assert(o.p2 = 398);
  assert(o.p3.SequenceEqual(Seq(1,2,3)));
end.