unit test0103u;

var a1 : array[1..3] of integer := (1,2,3);
    a2 : array[1..3] of real := (1.2,1.3,1.4);
    a3 : array[1..3] of string[5] := ('abc','def','ghj');
    a4 : array[1..3] of (red, green, blue, white) := (red,white,blue);
    a5 : array[1..3] of char := ('a','b','c');
    a6 : array['a'..'c'] of char := ('a','b','c');
    a7 : array[red..blue] of string := (a3[1],a3[2],a3[3]);
    
begin
end.