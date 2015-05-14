

procedure wrt(params pp:array of object);
var
i:integer;
begin
  for i:=0 to pp.Length-1 do
   writeln(pp[i].tostring);
end;

var
i:integer;
begin
 i:=1;
 writeln(i:10);
 wrt(1,2,3,'str',4,5);
 readln;
end.