
procedure Test;
var s : string := 'abc';
    arr1 : array of char := (s[1],s[2],s[3]);
    arr2 : array[1..3] of char := (s[1],s[2],s[3]);
var s2 := '123';
var arr3 : array of char := (s2[1],s2[2],s2[3]);
var arr4 : array[1..3] of char := (s2[1],s2[2],s2[3]);
    
begin
assert(arr1[0]='a');
assert(arr2[2]='b');
assert(arr3[0]='1');
assert(arr4[2]='2');    
end;

{procedure Test2;
var s : string := 'abc';
    arr1 : array of char := (s[1],s[2],s[3]);
    arr2 : array[1..3] of char := (s[1],s[2],s[3]);

procedure Nested;
    
begin
assert(arr1[0]='a');
assert(arr2[2]='b');
//var s2 := '123';
var arr3 : array of char := (s[1],s[2],s[3]);
var arr4 : array[1..3] of char := (s[1],s[2],s[3]);
assert(arr3[0]='1');
assert(arr4[2]='2');    
end;
    
begin
assert(arr1[0]='a');
assert(arr2[2]='b');
var s2 := '123';
var arr3 : array of char := (s2[1],s2[2],s2[3]);
var arr4 : array[1..3] of char := (s2[1],s2[2],s2[3]);
assert(arr3[0]='1');
assert(arr4[2]='2');    
end;}

var s : string := 'abc';
    arr1 : array of char := (s[1],s[2],s[3]);
    arr2 : array[1..3] of char := (s[1],s[2],s[3]);
var s2 := '123';
var arr3 : array of char := (s2[1],s2[2],s2[3]);
var arr4 : array[1..3] of char := (s2[1],s2[2],s2[3]);
    
begin
assert(arr1[0]='a');
assert(arr2[2]='b');
assert(arr3[0]='1');
assert(arr4[2]='2');
Test;
//Test2;   
end.