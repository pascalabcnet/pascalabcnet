var a := ArrGen(10,x->x);
var a2 := ArrGen(10,x->x*x);
var i: integer := 5;
var a3 := ArrGen(10,x->x+i);
var a4 := ArrGen(10,x->x);
    a5 := ArrGen(10,x->a[x]+1);
begin
  assert(a[9]=9);
  assert(a2[9]=81);
  assert(a3[9]=14);
  assert(a4[9]=9);
  assert(a5[9]=10);
end.