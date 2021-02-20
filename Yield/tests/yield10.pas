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

private
  function sin(x: real): real;
  begin
    result := 83.9;
  end;
end;

type A = class(R)
  testField: real := 23.3;

  function Gen(n: integer): sequence of real;
  var j,k: real;
  begin
    //yield testField;
    //yield testR;
    //msin;
    //yield mcos;
    //yield cos(3.14);
    yield sin(3.14);
    //yield jgdFail;
  end;
end;

begin
  var t := new A();
  foreach var x in t.Gen(5) do
    Print(x);
end.