begin
   var x: integer := 5;
   var a: array of integer := (x,x,x);
   var f: IntFunc := y->y+a[0]+x;
   writeln(f(1));
   writeln(a[2]);
end.