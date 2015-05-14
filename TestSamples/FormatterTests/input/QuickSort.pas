// Быстрая сортировка Ч. Хоара
const Sz=100;

type IArr=array [1..Sz] of integer;

procedure swap(var a,b: integer);
var v: integer;
begin
  v:=a;
  a:=b;
  b:=v
end;

procedure Print(const A: IArr; n: integer);
var i: integer;
begin
  for i:=1 to n do
    write(A[i],' ');
  writeln;
end;

procedure FillRandom(var A: IArr; n: integer);
var i: integer;
begin
  for i:=1 to n do
    A[i]:=random(100);
end;

procedure QuickSort(var A: IArr; n: integer);
  procedure sort(l,r: integer);
  var
    i,j: integer;
    w,x: integer;
  begin
    i:=l; j:=r;
    x:=A[(l+r) div 2];
    repeat
      while A[i]<x do Inc(i); // ищем первый элемент >= x
      while A[j]>x do Dec(j); // ищем последний элемент <= x
      if i<=j then
      begin
        swap(A[i],A[j]);
        Inc(i);
        Dec(j);
      end;
    until i>j;
    if l<j then sort(l,j);
    if i<r then sort(i,r)
  end;

begin
  sort(1,n)
end;

const n=30;

var A: IArr;

begin
  cls;
  FillRandom(A,n);
  writeln('До сортировки: ');
  Print(A,n);
  writeln('После сортировки: ');
  QuickSort(A,n);
  Print(A,n);
end.
