type TRec = record
a : array[1..3] of integer;
end;

var rec : TRec;
    
begin
rec.a[1] := 23;
var rec2 : TRec := rec;
assert(rec2.a[1] = 23);
rec2.a[1] := 15;
var arr : array of TRec := (rec,rec2);
assert(arr[0].a[1]=23);
assert(arr[1].a[1]=15);
end.