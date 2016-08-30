var a := 1;

function f: sequence of integer;
begin
  var ii: integer->integer := x->x;
  for var i:=1 to 5 do
    yield SeqGen(5,x->x+a).First;
end;

begin
  Assert(f.Println.SequenceEqual(Seq(1,1,1,1,1)));
end.