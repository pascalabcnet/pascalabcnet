// Все возможные способы инициализации поцедурной переменной
// Процедурный тип реализован через делегаты .NET, для него доступны операции +=, -=
var p: procedure;

procedure pp;
begin
  writeln('Вызов обычной процедуры');
end;

type 
  A = class
    x: integer;
    constructor Create(xx: integer);
    begin
      x := xx;
    end;
    procedure pp;
    begin
      writeln('Вызов метода класса, значение поля равно ',x);
    end;
    class procedure ppstatic;
    begin
      writeln('Вызов статического метода класса');
    end;
  end;

begin
  p := pp;
  var a1: A := new A(5);
  p += a1.pp;
  p += A.ppstatic;
  p;
end.