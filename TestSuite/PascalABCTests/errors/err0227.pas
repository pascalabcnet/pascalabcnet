type
  Person = class
  
  end;
  Student = class(Person)
  public 
    i: integer;
  end;

begin
  var persons := Arr(new Person, new Person);
  foreach x: Student in persons do
    Print(x.GetType);
end.