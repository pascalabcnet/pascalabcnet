type
  R1 = record
    a: array[1..1] of byte;
  end;

procedure Test(r: R1);
begin
  r.a[1] := 2;
  assert(r.a[1] = 2);
end;

var r2 := new R1;

begin
  var r:=new R1;
  r.a[1] := 2;
  assert(r.a[1] = 2);
  Test(new R1);
  r2.a[1] := 2;
  assert(r2.a[1] = 2);
end.