function Gen(): sequence of integer;
begin
   for var i:=1 to 10 do
   begin
     yield i;
     yield i*10;
   end;
end;

begin
   var q := Gen();
   q.Println;
end.