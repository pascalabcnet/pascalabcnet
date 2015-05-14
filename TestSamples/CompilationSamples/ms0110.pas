uses System.Collections.Generic;

type Person = record
  name: string;
  age: integer;
  constructor Create(nm: string; ag: integer);
  begin
    name := nm;
    age := ag
  end;
end;

var a : List<Person>;
//    p:=new Person('Иванов',20);

begin
  a := new List<Person>;
  //a.Add(p);
  a.Add(new Person('Иванов',20));
  //write(a[0].name);
  writeln(a[0].name);
end.