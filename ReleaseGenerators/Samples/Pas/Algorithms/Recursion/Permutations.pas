// Все перестановки
const n = 4;

var a: array of integer;

procedure Perm(m: integer);
begin
  if m=1 then
    a.Println;
  for var i:=0 to m-1 do
  begin
    Swap(a[i],a[m-1]); // ставим каждый на место последнего
    Perm(m-1);
    Swap(a[i],a[m-1]);
  end;  
end;

begin
  a := Range(1,n).ToArray; // заполнение массива a числами от 1 до n
  Perm(n);
end. 