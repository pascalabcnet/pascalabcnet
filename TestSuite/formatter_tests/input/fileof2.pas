//type TArr = array[1..4] of real;
//     TMatr = array[1..3,1..3] of integer;
     
type TRec = record
             a : integer;
             b : array[1..4] of real;
             c : array[1..3,1..3] of integer;
            end;
            
var f : file of TRec;
    rec,rec2 : TRec;
    
begin
rec.a := 3; rec.b[2] := 2.4; rec.c[1,2] := 11;
Assign(f,'test2.dat');
Rewrite(f);
Write(f,rec);
Write(f,rec);
Close(f);
Reset(f);
Read(f,rec2);
assert(rec2.a = 3);
assert(rec2.b[2] = 2.4);
assert(rec2.c[1,2] = 11);
Close(f);
Reset(f);
Seek(f,1);
Read(f,rec2);
assert(rec2.a = 3);
assert(rec2.b[2] = 2.4);
assert(rec2.c[1,2] = 11);
Close(f);
end.