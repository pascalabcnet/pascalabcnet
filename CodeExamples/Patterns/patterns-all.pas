type
  Person = auto class
    name: string;
    age: integer;
  end;

type
  Student = class(Person);

begin
  var b1 := new Student('asd', 30) is Person(var c);
  writeln(c);
  
  var b2 := 1 is integer(var d);
  writeln(d);
  
  var arr := Arr(1, 3, 5, 7, 9);
  
  var l := new List<integer>(arr);
  var b4 := l is List<integer>(var f);
  writeln(f);
  
  var delegate: Func<word, char> := PABCSystem.ChrUnicode;
  var b5 := delegate is Func<word, char>(var g);
  writeln(g);
end.