uses
  System, System.Net, System.Threading.Tasks, System.Collections.Generic;

type
  ListSimple = class
    l := new List<int64>();
    procedure AddSimple(N: int64);
    begin
      var Result := True;
      for var i: int64 := 2 to round(sqrt(N)) do
        if N mod i = 0 then
        begin
          Result := False;
          break;
        end;
      // lock не позволяет нескольким потокам одновременно выполнять эту секцию. 
      // Это обязательно, иначе если несколько потоков одновременно будут пытаться добавить значение в список, список может оказаться испорченным
      // Секция, охраняемая lock, называется критической
      lock l do 
        if Result then
          l.Add(N);
    end;
  end;

var hh := 1000;

begin
  var ls := new ListSimple;
  
  var ii: int64 := 1265713689181;
  
  Parallel.For(ii,ii+hh,ls.AddSimple);
  
  writeln(ls.l.Count);
  // ls.l := ls.l.OrderBy(x->x).ToList(); - это можно включить для проверки того, что в обоих списках - одинаковые простые числа
  for var i:=0 to ls.l.Count-1 do
    write(ls.l[i],' ');
  // Надо обратить внимание, что выводится неупорядоченная последовательность простых чисел !
  
  writeln;  
  writeln('Время выполнения = ',Milliseconds);  
  
  // То же, но непараллельно
  var ls1 := new ListSimple;

  var t := Milliseconds;  
  for var i:=ii to ii+hh-1 do
    ls1.AddSimple(i);
  writeln(ls1.l.Count);
  for var i:=0 to ls1.l.Count-1 do
    write(ls1.l[i],' ');
  writeln;  
  writeln('Время выполнения = ',Milliseconds-t);  
  
  // Проверка того, что списки простых как множества совпадают
  writeln('Количество элементов в разности множеств = ',ls.l.Except(ls1.l).Count());
end.