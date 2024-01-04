##
var c := (var a := (var b := 5 + 1) + 2);
Assert(a = 8);
Assert(b = 6);
Assert(c = 8);
Print(a,b,c);
var lst := |(var yyy := 2),yyy+1|;
Print(lst);
Assert(lst.SequenceEqual(Arr(2,3)));
var a1 := (var b1 := 5) ** 2;             
var a2 := Sqrt((var b2 := 5));                  
var res := Arr(1,2,3).Sum(x -> begin var d := (var b := 1); Result := d; end);
res.Print;
Assert(res = 3);
