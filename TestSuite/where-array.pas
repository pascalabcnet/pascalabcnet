type
  r1 = record end;
  
  t1<T> = class
    where T: System.Array;
  end;
  
procedure p1<T>; where T: System.Array;
begin end;

begin
  
  p1&<array of byte>;
  p1&<array of r1>;
  
  new t1<array of byte>;
  new t1<array of r1>;
  
end.