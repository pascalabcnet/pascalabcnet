uses System.IO;

uses UnitClass;

var aa: real := 555.0;


type R = class
  testR: real := 77.3;
  testI: integer;

  property sin: real read testR;

  function sin2(x: real): real;
  begin
    result := 83.9;
  end;
private
  testRr: real := 23.3;
end;

type A = class(UnitClassTest)
  (*constructor Create;
  begin
    inherited Create('hz');
  end;

  constructor Create(path: string);
  begin
    inherited Create(path);
  end;*)

  function Gen(n: integer): sequence of real;
  var j,k: real;
  begin
    //var x := BaseStream;
    //var x := 56.4;
    //var y := x;
    yield testField;
    //yield Peek();
  end;
end;

begin
  var t := new A();
  foreach var x in t.Gen(5) do
    Print(x);
end.