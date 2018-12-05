type
  TClass0 = record
    protected auto property p1: byte;
  end;
  
  TClass1 = record
    public auto property p2: integer;
  end;
  
  TClass2 = record
    public auto property p3: sequence of integer;
  end;

begin
  var o1 := new TClass0();
  var o2 := new TClass1();
  var o3 := new TClass2();
  o1.p1 := 123;
  o2.p2 := 398;
  o3.p3 := Seq(1, 2, 3);
  assert(o1.p1 = 123);
  assert(o2.p2 = 398);
  assert(o3.p3.SequenceEqual(Seq(1, 2, 3)));
end.