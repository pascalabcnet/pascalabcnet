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
  var j := n;
  var jj : real := j;
  begin
    //begin
      var x := 777;
      //yield xx;
    //end;
    //begin
      //var x := 89.3;
      //begin
      //  var x := 9;
      //end;
      yield jj;
      for var x := 1 to j do
      begin
        yield x;
      end;
      
    //end;
  end;
end;

begin
  var t := new A();
  foreach var x in t.Gen(5) do
    Print(x);
end.