type
  Person = class
    name: string;
    age: integer;
    
    class function Unapply(p: Person; name: string; age: integer): boolean;
    begin
      name := p.name;
      age := p.age;
      result := true;
    end;
  end;

type
  Cartesian = class
    x: integer;
    y: integer;
  end;

type
  Polar = class
    
    class function Unapply(c: Cartesian; var R: real; var Theta: real): boolean;
    begin
      R := Sqrt(c.X * c.X + c.Y * c.Y);
      Theta := ArcTan(c.Y / c.X);
      result := not (c.X = 0) and not (c.Y = 0);
    end;
  end;

begin
  var p := new Person;
  if p is Person(var name, var age) then
    Print(name, ' ', age);
end.