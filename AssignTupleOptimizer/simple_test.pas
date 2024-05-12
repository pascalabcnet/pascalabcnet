var A := 3;
var B := 2;
var C := 4;
var D := 5;

function test2() : Integer;
var a := 3;
procedure p(var b :Integer);
begin
   
  (a, b) := (b, D);
  Print(b);
  Print(a);
end;
begin
  p(a);
end;

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

	(a, B, c, D) := (c, A, D, b);
end;

begin 

	var a := 3;
	var b := 4;
	var c := 5;
	var d := 6;
	
	(a, b) := (c, d); 

	(a, b) := (b, a);
end.