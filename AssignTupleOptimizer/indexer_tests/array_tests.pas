begin

	var a : array [1..2] of integer := (0, 1);
	var b : array [1..2] of integer := (4, 3);

	(a[0], a[1]) := (a[1], a[0]);
	

	(a[0], a[1], a[2]) := (a[1], a[2], a[0]);
	
end.