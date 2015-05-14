procedure Test;
var a : array of real := (2.4,2.5,2.6);
procedure Nested;
//var a : array of real := (2.4,2.5,2.6);
begin
 writeln(a[1]);
end;

begin
 Nested;
 writeln(a[1]);
end;


var a : array of real := (2.4,2.5,2.6);
    b : array of set of char := (['a','b'],['c','d'],['f','g']);
    a1 : array[1..3] of real := (2.4,2.5,2.6);
    b1 : array[1..3] of set of char := (['a','b'],['c','d'],['f','g']);
    c : array of array of real := ((2,3,4),(1.2,3.1,2.4),(5.5,6.7,7.8));
    
begin
 Test;
 writeln(a[1]);
 writeln(b[1]);
 writeln(a1[1]);
 writeln(b1[1]);
end.