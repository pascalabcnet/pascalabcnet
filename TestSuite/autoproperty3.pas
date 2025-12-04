type
  TBaseClass = abstract class
    auto property p1: byte;
  end;
  
  TClass1 = class(TBaseClass)
    auto property p2: integer;
  end;
  
  TClass2 = class(TClass1)
    auto property p3: sequence of integer;
  end;

begin
  var o: TClass2 := new TClass2;
  o.p1 := 2;
  o.p2 := 398;
  o.p3 := Seq(1, 2, 3);
  assert(o.p1 = 2);
  assert(o.p2 = 398);
  assert(o.p3.SequenceEqual(Seq(1, 2, 3)));
end.