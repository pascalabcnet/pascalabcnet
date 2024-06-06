type
	A = class
		p : Integer;
	end;

	B = class	
		f1 : A;

	end;

	C = class(A)
		f2 : B;
	end;

begin

	var aa := new A();
	var bb := new B();
	var cc := new C();

	aa.p := 2;
	var aa1 := new A();
	aa1.p := 3;

	bb.f1 := aa;
	cc.f2 := bb;
  MillisecondsDelta;
	(aa.p, cc.f2.f1.p) := (bb.f1.p, aa1.p);
	println(MillisecondsDelta);
end.