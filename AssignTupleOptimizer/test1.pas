type
	A = class
		p : Integer;
		q : Integer;
	end;

	B = class	
		f1 : A;

	end;

	C = class(A)
		f2 : B;
	end;

begin
var a : array [1..2] of integer := (0, 1);
	var b : array [1..2] of integer := (4, 3);

	(a[0], a[1]) := (a[1], a[0]);
	var j := 1;
	var i := 1;
	var k := 0;
	var aa := new A();
	var bb := new B();
	var cc := new C();
	var a := 1;
	var c := 1;
	aa.q := 3;
	aa.p := 2;
	var aa1 := new A();
	aa1.p := 3;
	aa1.q := 5;
	var aa2 := new A();
	bb.f1 := aa;
	cc.f2 := bb;

	(a[j], a[i], aa1.p, bb.f1.p, a, aa2.p) := (aa.p, a[k], cc.f2.f1.p, a[j], a[k], b);

end.