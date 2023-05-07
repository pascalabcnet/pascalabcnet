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
  var p := new Person('John', 25);
  var s := '';
  var i := 0;
  if p is Person(var name, var age) then
  begin
    s := name;
    i := age;
  end;
  
  assert(s = 'John');
  assert(i = 25);
  
  s := '';
  i := 0;
  
  match p with
    Person(name, age):
    begin
       s := name;
       i := age;
    end;
  end;
  
  assert(s = 'John');
  assert(i = 25);
end.