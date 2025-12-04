procedure Test(params arr: array of integer);
begin
  assert(arr[0]=1);
  assert(arr[1]=2);
end;

function Testsum(params arr: array of integer): integer;
begin
  Result := arr[0]+arr[1];
end;

procedure Test2;
var i2 := Testsum(1,2);
procedure Nested;
begin
  Test(1,2);
  var i := Testsum(1,2);
  assert(i = 3);
  assert(i2 = 3);
end;
begin
  Test(1,2);
  Nested;
  var i := Testsum(1,2);
  assert(i = 3);
  assert(i2 = 3);
end;

type TClass = class
i := Testsum(1,2);
constructor;
begin
  assert(i = 3);
  Test(1,2);
end;
end;

var i2 := Testsum(1,2);

begin
  Test(1,2);
  Test2;
  var i := Testsum(1,2);
  assert(i = 3);
  assert(i2 = 3);
  var obj := new TClass;
end.