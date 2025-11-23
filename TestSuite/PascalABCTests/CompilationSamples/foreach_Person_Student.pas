type
  Person = class
  
  end;
  Student = class(Person)
  public 
    i: integer;
  end;

begin
  var persons := Arr(new Student, new Student);
  foreach x: Person in persons do
    Print(x.GetType);
end.