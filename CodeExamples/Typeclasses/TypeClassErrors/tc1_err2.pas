uses TypeClasses;

type 
  Person = auto class
    name: string;
    age: integer;
  end;
  
  Eq[Person] = instance
    function operator=(x, y: Person): boolean := x.name = y.name;
  end;   

  Ord[Person] = instance
    //function operator=(x, y: Person): boolean := x.name = y.name;
    function Less(x, y: Person): boolean := x.name < y.name;
  end;   
  
procedure ppp<T>(p1,p2: T); where Ord[T];
begin
  Print(p1=p2);
  Print(p1<p2);
end;

begin
  var p1 := new Person('Ivanov',20);
  var p2 := new Person('Ivanov',20);
  ppp(p1,p2);
end.