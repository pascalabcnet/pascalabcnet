procedure p1();
begin
  {$omp parallel for}
  for var i:=1 to 10 do
  begin
    var a: array of integer;
    //a := ArrGen(i,x->x);
  end;
  {$omp parallel sections}
  begin
    var a: array of integer;
    a := ArrGen(2, x -> 0);
  end;
end;

begin
end.