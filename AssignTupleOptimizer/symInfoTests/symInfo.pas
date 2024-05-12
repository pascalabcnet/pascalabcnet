var p : Integer := 3;
type
	A = class
		p : Integer;
		procedure test();
		begin
			p := 4;
		end;

		procedure test4();
		
	end;

	B = class	
		procedure test1();
		begin
			p:=4;
		end;

	end;

	C = class(A)
		procedure test2();
		begin
			p:=4;
		end;
	end;

	procedure A.test4();
	begin
		p := 4;
	end;

begin
	p := 3;
end.