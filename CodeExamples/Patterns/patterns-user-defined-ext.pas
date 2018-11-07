// Деконструктор как метод расширения

procedure Deconstruct(Self: integer; var a: integer; var b: integer); extensionmethod;
begin
  a := Self div 10;
  b := Self mod 10;
end;


begin
  var i := 25;
  if i is integer(var a, var b) then
    Println(a,b);  
end.