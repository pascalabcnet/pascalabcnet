// В массиве B задано множество элементов. Вывести все его подмножества
const Sz=100;

type IArr=array [1..Sz] of integer;

procedure PrintArr(const A: IArr; n: integer);
var i: integer;
begin
  for i:=1 to n do
    write(A[i],' ');
  writeln;
end;

procedure TrySolve(const B: IArr; nb: integer);
var
  Subset: IArr;
  ns: integer;

  procedure TrySolve0(i: integer);
  begin
    if i>nb then PrintArr(Subset,ns)
    else
    begin
      TrySolve0(i+1); // попробовать не взять i-тый элемент
      Inc(ns); Subset[ns]:=B[i]; TrySolve0(i+1); Dec(ns); // попробовать взять i-тый элемент
    end;
  end;
  
begin
  TrySolve0(1);
end;

procedure FillArr(var B: IArr; var n: integer);
begin
  n:=5;
  B[1]:=5; B[2]:=3; B[3]:=8; B[4]:=13; B[5]:=15;
end;

var
  B: IArr;
  nb: integer;

begin
  cls;
  FillArr(B,nb);
  TrySolve(B,3);
end.
