function Gen(n: integer): sequence of real;
begin
   begin
     var x := 2;
     yield x;
     begin
       var y := 666;
       yield x + z;
     end;
   end;
   begin
     var x := 3.8;
     yield x;
   end;
   var x := 55;
   yield x;
end;

begin
   var q := Gen(666);
   q.Println;
end.