type A<T> = class
  testField: T;
  
  function Gen: sequence of T;
  begin
    yield testField;
  end;

end;


begin
  var q := new A<integer>();
  q.testField := 777;
  q.Gen.Println;
end.