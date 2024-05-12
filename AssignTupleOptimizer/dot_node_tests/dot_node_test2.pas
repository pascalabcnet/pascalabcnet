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

	(aa.p, cc.f2.f1.q, aa.q, aa1.p) := (bb.f1.p, aa1.p, cc.f2.f1.q, i);

end.