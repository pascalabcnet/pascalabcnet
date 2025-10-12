// Все возможные способы инициализации поцедурной переменной
// Процедурный тип реализован через делегаты .NET, для него доступны операции +=, -=

procedure pp;
begin
  Println('Вызов обычной процедуры');
end;

type 
  A = class
  private
    x: integer;
  public  
    constructor Create(xx: integer);
    begin
      x := xx;
    end;
    procedure pp;
    begin
      Println('Вызов метода класса, значение поля равно',x);
    end;
    class procedure ppstatic; 
    begin
      Println('Вызов классового метода класса');
    end;
  end;

begin
  var p: procedure;

  p := pp;
  var a1: A := new A(5);
  p += a1.pp;
  p += A.ppstatic;
  p;
  Println;
  p -= pp;
  p;
end.