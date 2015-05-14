unit test0141u;

const 
  s1 : set of 1..7 = [1,2,6];
  s2 : set of 1..3 = s1;
  s3 : string[5] = 'abcd';
  s4 : string[3] = s3;
  
var 
  v1 : set of 1..7 := [1,2,6];  
  v2 : set of 1..3 := v1;
  v3 : string[5] := 'abcd';
  v4 : string[3] := v3;
 
procedure Test;
const 
  s1 : set of 1..7 = [1,2,6];
  s2 : set of 1..3 = s1;
  s3 : string[5] = 'abcd';
  s4 : string[3] = s3;
  
var 
  v1 : set of 1..7 := [1,2,6];  
  v2 : set of 1..3 := v1;
  v3 : string[5] := 'abcd';
  v4 : string[3] := v3;
  
begin
assert(s2=[1,2]);
assert(v2=[1,2]);
assert(s4='abc');
assert(v4='abc');
end;

procedure Test2;
procedure Nested;
const 
  s1 : set of 1..7 = [1,2,6];
  s2 : set of 1..3 = s1;
  s3 : string[5] = 'abcd';
  s4 : string[3] = s3;
  
var 
  v1 : set of 1..7 := [1,2,6];  
  v2 : set of 1..3 := v1;
  v3 : string[5] := 'abcd';
  v4 : string[3] := v3;
  
begin
assert(s2=[1,2]);
assert(v2=[1,2]);
assert(s4='abc');
assert(v4='abc');
end;
begin
Nested;
end;

begin
assert(s2=[1,2]);
assert(s4='abc');
assert(v2=[1,2]);
assert(v4='abc');
Test;
Test2;
end.