type TRec = record
             a : string;
             b : array[1..4] of integer;
            end;
            
var p : ^TRec;
    arr : ^array[1..5] of integer;
    pset,pset2 : ^set of byte;
    
begin
New(arr);
arr^[3] := 123;
assert(arr^[3] = 123);
Dispose(arr);
New(pset);
Include(pset^,3);
Include(pset^,5);
assert(3 in pset^);
assert(5 in pset^);
New(pset2);
pset2^ := [1,2,4];
pset^ := pset2^;
assert(pset^ = [1,2,4]);
Include(pset^,7);
assert(pset2^ = [1,2,4]);
Dispose(pset);
Dispose(pset2);
end.