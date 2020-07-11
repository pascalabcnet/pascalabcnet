function f1(o: object): sequence of object;
begin
  
  var c := o is object(var a);
  //if o is object(var a) then
  //begin
  //  var b := a;
  //  Writeln(a); // object
  yield a;
  Assert(a <> nil); // nil
  //  Writeln(b); // object
  //  b := b;
  //end;
end;

begin
  foreach var o in f1(new object) do
    
end.