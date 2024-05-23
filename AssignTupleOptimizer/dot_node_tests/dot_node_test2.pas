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
	var j := 3;
	var i := 3;
	var aa := new A();
	var bb := new B();
	var cc := new C();

	aa.q := 3;
	aa.p := 2;
	var aa1 := new A();
	aa1.p := 3;
	aa1.q := 5;

	bb.f1 := aa;
	cc.f2 := bb;

	(aa.p, cc.f2.f1.q, aa.q, aa1.p, a[0], a[1]) := (bb.f1.p, aa1.p, cc.f2.f1.q, i, a[1], j);

end.