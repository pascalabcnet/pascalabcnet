type TSet = set of 1..5;

procedure Test(params arr : array of TSet);
begin
assert(arr[1]=[1..5]);
end;

{procedure Test3(params arr : array of string[3]);
begin
assert(arr[1]='abc');
end;}

procedure Test2(params arr : array of TSet);
procedure Nested;
begin
assert(arr[1]=[1..5]);
end;

begin
assert(arr[1]=[1..5]);
Nested;
end;

begin
Test([1..3],[1..7]);
Test2([1..3],[1..7]);
//Test3('ab','abcdef');
end.