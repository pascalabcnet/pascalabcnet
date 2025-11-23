type
  Person = auto class
    name: string;
    age: integer;
    
    class procedure Deconstruct(var age: integer);
    begin
      age := 666;
    end;
  end;


begin
  var p := new Person('Иванов',20);
  if p is Person(var age: integer) then
    Print(name);
end.