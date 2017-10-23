type
  Ir1=interface end;
  
  r1=record(Ir1) 
    i: integer;
  end;

procedure Test(i: Ir1);
begin
  assert(r1(i).i = 2);
end;

begin
  var a := new Ir1[1];
  var rrr: r1;
  rrr.i := 2;
  a[0] := rrr;
  var i1: Ir1 := rrr;
  assert(r1(a[0]).i = 2);
  assert(r1(i1).i = 2);
  Test(a[0]);
end.