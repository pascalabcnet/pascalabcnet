var s: set of 1..3;
    s1 : set of 1..3;
    arr : array of set of char;
    s2 : set of byte;
    i : integer;
    break : real; 
begin
  //s := [1..5];
  i := 4;
  Include(s2,i);
  writeln(s2);
  s := s1;
  //Include(s2,b);
  Include(s,2);
  Include(s,3);
  Include(s,5);
  //s1 := new TypedSet(1,4);
  //s1.IncludeElement(5);
  //writeln(s1.Contains(5));
  writeln(s);
end.