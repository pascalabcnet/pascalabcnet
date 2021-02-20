uses TypeClasses;

type 
  Person = auto class
    name: string;
    age: integer;
  end;
  
procedure ppp(p: Person); where Ord[Person];
begin
  
end;

begin
  var p1 := new Person('Ivanov',20);
  var p2 := new Person('Ivanov',20);
  Print(p1=p2);
end.