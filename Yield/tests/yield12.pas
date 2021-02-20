var aa: real := 555.0;

function mcos: real;
begin
  result := 99.6;
end;

procedure msin;
begin
end;

type R = class
  testR: real := 77.3;
  testI: integer;

  //property sin: real read testR;

//private
  function sin2(x: real): real;
  begin
    result := 83.9;
  end;
end;

type A = class(R)
  testField: real;

  function Gen(n: integer): sequence of real;
  var j := n;
  begin
    yield testField;
    yield sin(3.14);
    yield j;
  end;
end;

begin
  var t := new A();
  foreach var x in t.Gen(5) do
    Print(x);
end.