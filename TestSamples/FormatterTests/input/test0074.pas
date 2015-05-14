uses test0074u;

var a : TArr;
    b : TArr2;
    c : TSet;
    d : TFile;
    rec : TRec;
    
begin
a[1].a := 3;
assert(a[1].a = 3);
Test(a);
SetLength(b,4);
Test(b);
rec.a := 11;
Include(c,rec);
assert(rec in c);
Test(c);
Test(d);
end.