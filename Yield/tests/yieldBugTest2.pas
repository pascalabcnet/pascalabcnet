function Gen(n: integer): sequence of integer;
begin
   begin
   yield 666;
   end;
end;

begin
   var q := Gen(666);
   q.Println;
end.