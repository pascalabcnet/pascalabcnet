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

  function sin2(x: real): real;
  begin
    result := 83.9;
  end;
end;

type A = class(R)
  testField: real;

  function Gen(n: integer): sequence of real;
private
  function Lol: real;
end;

function A.Lol: real;
begin
end;

function A.Gen(n: integer): sequence of real;
  var j := n;
  var jj : real := j;
  begin
      yield jj;
      for var x := 1 to j do
      begin
        yield x;
      end;
  end;

begin
  var t := new A();
  foreach var x in t.Gen(5) do
    Print(x);
end.