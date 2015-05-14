type TArr = array[1..3] of integer;
     TSet = set of char;
     
type TRec = record
a : TArr;
b : string[6];
end;

var prec : ^TRec;
    rec : TRec;
    arr : TArr;
    set1 : TSet:=['a','b','c'];
    parr : ^TArr;
    pset : ^TSet;
    
begin
rec.a[1] := 3;
prec := @rec;
assert(prec^.a[1]=3);
New(prec);
prec^.b := 'privet';
assert(prec^.b='privet');
arr[1] := 2;
parr := @arr;
assert(parr^[1]=2);
pset := @set1;
assert('b' in pset^);
end.