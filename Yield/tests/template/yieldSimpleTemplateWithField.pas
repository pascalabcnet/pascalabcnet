type B<T> = class
  testBaseField: T;
end;

type A<T> = class(B<T>)
  testField: T;
  
  function Gen: sequence of T;
  begin
    yield testBaseField;
  end;

end;


begin
  var q := new A<integer>();
  q.Gen.Println;
end.