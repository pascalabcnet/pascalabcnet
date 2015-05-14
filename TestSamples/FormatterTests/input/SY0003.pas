type
  ccc = class(System.Collections.ArrayList)
  end;
  
var
  arr: ccc;
  
begin
  arr := new ccc;
  arr.Add(3);
  writeln(arr[0]);
  readln;
end.