type
  Person = class
    name: string;
    age: integer;
    
    class function Deconstruct(p: Person; name: string; age: integer): boolean;
    begin
      name := p.name;
      age := p.age;
      result := true;
    end;
  end;


begin
  var p := new Person;
  if p is Person(var name,var age) then
    Print(name, ' ', age);
end.