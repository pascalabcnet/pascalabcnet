// Switch-statement 

begin
  var a := new List<integer>(arr(1, 2));
  match a with
    integer(var c) when c = 3: write(1);
    string(var s) when not string.IsNullOrEmpty(s): write(2);
    List<integer>(var l) when l.Count > 0: write(3)
  end;
end.

// User defined

type
  Person = class
    name: string;
    age: integer;
    
    function Deconstruct(p: Person; name: string; age: integer): boolean;
    begin
      name := p.name;
      age := p.age;
      result := true;
    end;
  end;


begin
  var p := new Person;
  if p is Person(var name, var age) then
    Print(name, ' ', age);
end.

// Recursive patterns

begin
  if (tree is BinOp(IntConst(var ln), IntConst(var rn))) then
    tree = new IntConst(ln + rn);
end.