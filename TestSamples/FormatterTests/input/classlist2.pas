// Демонстрация класса односвязного списка целых с внутренним итератором
//using System;

type ttest = class
//a : array[2..6] of real;
//constructor Create;overload;
constructor Create(s : string);overload;
end;
    {
constructor ttest.Create;
begin
 writeln(2);
end;
     }
constructor ttest.Create(s : string);
procedure nested;
begin
 //writeln(s);a[5] := 321;
 //writeln(a[5]);
end;
begin
 //nested;
end;

var //l: IntList;
   cls : TTest;

begin
 cls := ttest.Create('ok');
  {l:=IntList.Create;
  l.Add(1);
  l.Add(2);
  l.Add(3);
  l.Add(7);
  l.Add(5);
  l.Add(6);

  l.Print;
  writeln;

  l.Reset;
  while not l.EndOfList do
  begin
    Console.Write(l.CurrentValue.toString+' '.tostring);
    l.Next
  end;
  writeln;
  l.Destroy;    }
end.
