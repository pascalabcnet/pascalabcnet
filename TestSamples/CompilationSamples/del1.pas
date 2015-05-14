//Тест делегатов с использованием PCU
uses
  del1_u, del1_u2;

procedure MyProc(y: integer);
begin
  writeln(y);
end;

procedure MyProc2(y: integer);
begin
  writeln(y*2);
end;

begin
  PrintEvent += MyProc;
  PrintEvent += MyProc2;
  PrintEvent += MyProc3;
  PrintEvent(20); //Выведет 20, 40, 60
  readln;
end.
