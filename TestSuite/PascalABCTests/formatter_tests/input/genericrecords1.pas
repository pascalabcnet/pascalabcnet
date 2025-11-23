type TRec<T> = record
a, b : T;
end;

var rec : TRec<integer>;
    f : file of TRec<integer>;
    
begin
Assign(f,'test.dat');
Rewrite(f);
rec.a := 1;
rec.b := 2;
Write(f,rec);
rec.a := 3;
rec.b := 4;
Write(f,rec);
Close(f);
Reset(f);
Read(f,rec);
assert(rec.a = 1);
assert(rec.b = 2);
Read(f,rec);
assert(rec.a = 3);
assert(rec.b = 4);
Close(f);
end.