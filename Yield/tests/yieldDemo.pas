type BaseClass = class
  testBaseField: real := 77.3;

  function testBaseFunction(x: real): real;
  begin
    result := 83.9;
  end;
end;

type A = class(BaseClass)
  testField: real := 777.7;

  function Gen(testFormalParam: integer): sequence of real;
  var testLocalVariable := testFormalParam;
  begin
      var testLocalVariable_2 : real := testLocalVariable + 33.3;

      yield testLocalVariable;
      yield testLocalVariable_2;

      yield testFormalParam;

      yield testField;
      yield testBaseField;
      yield testBaseFunction(9);

      yield sin(3.14);

      for var x := 1 to testLocalVariable do
      begin
        yield x;
      end;
  end;
end;

begin
  var t := new A();
  foreach var x in t.Gen(5) do
    Print(x);
end.