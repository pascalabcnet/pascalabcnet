uses TypeClasses;

// Плохое сообщение об ошибке
procedure ppp<T>(p: T); where Num[T];
begin
  Print(Num[T].Abs(p));
end;

begin
  
end.