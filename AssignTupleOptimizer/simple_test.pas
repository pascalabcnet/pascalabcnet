

procedure test();
begin
	var a := 3;
	var b := 4;
	var c := 5;
	var d := 6;
	
	(a, b) := (c, d); 

	(a, b) := (b, a);

	(A, b) := (c, d);

	(A, b) := (B, a);
end;

begin 

	var a := 3;
	var b := 4;
	var c := 5;
	var d := 6;
	
	(a, b) := (c, d); 

	(a, b) := (b, a);
end.