type Matr = array[1..3,1..3] of integer;

function Func : Matr;
begin
  
end;

procedure Test(const m : Matr);
begin
  Test(m);
end;

begin
Test(Func());  
end.