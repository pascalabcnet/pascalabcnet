type TRec = record
a: integer;
b: byte;
r: real;
rec: record
      c: char;
      f: single;
     end;
str: string;
end;

//const 
//  rec: TRec = (a:3;b:2;r:2.6);
var rec: TRec := (a:3;b:2;r:2.6;rec:(c:'a';f:2.3);str:'Hello!');
    rec1: TRec;
begin
  rec1 := rec;
  assert(rec1.a=3);
  assert(rec1.b=2);
  assert(rec1.r=2.6);
  assert(rec1.rec.c='a');
  assert(rec1.str='Hello!');
end.