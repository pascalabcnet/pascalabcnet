begin

	var a : array [1..2] of integer := (0, 1);
	var b : array [1..2] of integer := (0, 1);


	(a, a[1]) := (b, b[0]);

end.