begin
   var f: IntFunc := x->x*4;
   var g: IntFunc := x->x + f(0);
   assert(f(1) = 4);
end.