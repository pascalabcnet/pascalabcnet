uses u_test0065,u2_test0065;

type TRec = record
      a : TRec2;
      b : TRec3;
      
     end;

var rec : TRec;

begin
rec.a.c.d[1].f := 3.14;
assert(rec.a.c.d[1].f = 3.14);
rec.b.e := 5;
assert(rec.b.e = 5);    
end.