unit u_delegates4;
type TFunc = function(i: integer):integer;

function f1(i : integer) : integer;
begin
  Result := i;
end;

function f2(i : integer) : integer;
begin
  Result := i;
end;

const arr : TFunc = f1;
      arr2 : array[1..3] of TFunc = (f1,f2,f1);
      
begin
assert(arr(3)=3);
assert(arr2[2](2)=2);
end.