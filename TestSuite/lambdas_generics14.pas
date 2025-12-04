type
  t0 = class
    //constructor := exit; 
  end;
  
procedure p1<T>;
where T: t0, constructor;
begin
  var o: T;
  var p := procedure -> o := o;
end;

begin
  p1&<t0>;
end.