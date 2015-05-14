uses
  System.Collections;
  
var
  a, b: ArrayList;
    
begin
  a := new ArrayList;
  b := new ArrayList;
  a.Add(1);
  b.Add(3);
  b.Add(5);
  a.AddRange(b);
  writeln(a[0]);
  writeln(a[1]);
  writeln(a[2]);
  readln;
end.