type A = class
  testField: real;
  
  function Gen: sequence of real;
  begin
    yield testField;
  end;

end;


begin
  var q := new A();
  q.Gen.Println;
end.