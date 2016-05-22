
function Gen(n: integer): sequence of integer;
begin
   for var i:=1 to 10 do
     yield i;
   yield 666;
end;

begin
   var q := Gen(666);
   q.Println;
end.