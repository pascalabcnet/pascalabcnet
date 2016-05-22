function Gen(n: integer): sequence of integer;
begin
   if n=0 then
     yield 666
   else yield 777;
end;

begin
   var q := Gen(1);
   q.Println;
end.