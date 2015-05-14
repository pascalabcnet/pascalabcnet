type SomeClass<T> = class
  procedure SomeMethod(params someData: array of T); 
  begin
    assert(someData.Count = 0);  
  end;
  class function SomeFunc(params someData: array of T): T;
  begin
    
  end;
end;

procedure SomeProcedure<T>(someArgument: T);
begin
  var someObject := new SomeClass<T>();
  someObject.SomeMethod();
end;

function Test(params arr: array of integer): integer;
begin
  Result := arr.Sum();
end;

type TClass = class
a: integer := Test(1,2);
end;

begin
  SomeProcedure(1);
  var t := new TClass;
  assert(t.a = 3);
end.