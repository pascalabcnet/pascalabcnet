var z:=1;

type AAA = class
  y: integer := 1;
  function f(a: sequence of integer): sequence of integer;
  begin
    var k: integer = 1;
    foreach var x in a do
      yield x+y+z+k;
  end;
end;  

begin
  var a1 := new AAA();
  
  var q := a1.f(Range(1,4));
  
  var sq := Seq(4,5,6,7); 
  Assert(q.Println.SequenceEqual(sq));
  Assert(q.Println.SequenceEqual(sq));
end.