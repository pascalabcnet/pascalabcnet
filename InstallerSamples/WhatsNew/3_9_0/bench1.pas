uses Utils;

procedure Test1;
begin
  var n := 1000000;
  var s := 0.0;
  for var i:=1 to n do
    s += Sin(i);
end;

begin
  Benchmark(Test1).Println;

  var n := 100000000;
  var Sq := ArrRandomInteger(n,0,MaxInt-1);
  Benchmark(()->
  begin
    var Min := Sq.Min;
  end,1).Println;
end.