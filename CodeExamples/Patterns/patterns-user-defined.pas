type
  Person = class
    name: string;
    age: integer;
    
    constructor(name: string; age: integer);
    begin
      self.name := name;
      self.age := age;
    end;
    
    procedure Deconstruct(var name: string; var age: integer);
    begin
      name := self.name;
      age := self.age;
    end;
  end;


begin
  var p := new Person('Петр', 25);
  if p is Person(var name, var age) then
    Print(name, age);
end.