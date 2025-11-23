unit u_classvirtual1;
type TBase = class

constructor;
begin
  inherited;
end;

constructor Create(a : integer);
begin
  assert(a=3);
end;
   
procedure Test(a : integer); virtual;
begin
  assert(a=3);
end;
function Test2(var k : integer; params arr : array of integer) : integer;virtual;
begin
  k := 10;
  assert(arr[0]=3);
end;
procedure Test3(a : string[3]); virtual;
begin
  assert(a='abc');
end;

procedure Test4(a : char); virtual;
begin
  assert(a='k');
end;

class procedure Test5(a : set of byte);
begin
assert(a=[1,2,3]);
end;
end;

type TDer = class(TBase)
constructor(i : integer);
begin
inherited;
end;
procedure Test(a: integer); override;
begin
  inherited;
end;
function Test2(var k: integer; params arr: array of integer): integer; override;
begin
  inherited;
  assert(k=10);
end;
procedure Test3(a: string [3]); override;
procedure Test4(a: char); reintroduce;
begin
  inherited;
end;

class procedure Test5(a : set of byte);
begin
  inherited;
end;
end;
  
procedure TDer.Test3(a: string [3]);
procedure Nested;
begin
  inherited;
end;
begin
  inherited;
  Nested;
end;

begin
var t := new TDer(3);
t.Test(3);
var k : integer;
t.Test2(k,3,4,5);
t.Test3('abcd');
t.Test4('k');
TDer.Test5([1,2,3]);
end.