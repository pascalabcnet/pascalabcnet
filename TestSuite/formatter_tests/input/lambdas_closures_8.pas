begin
   var f: IntFunc := x->x*4;
   var g: IntFunc := x->x + f(0);
   f(1);
end.