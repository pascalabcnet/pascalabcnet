unit u_genericrecords2;
type TRec<T> = record
a : T;
end;

var f : file of TRec<integer>;
    rec,rec2 : TRec<integer>;
    
begin
Assign(f,'out.dat');
rec.a := 11;
Rewrite(f);
Write(f,rec);
Close(f);
Reset(f);
Read(f,rec2);
assert(rec2.a = 11);
Close(f);
end.