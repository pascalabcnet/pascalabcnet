// Пример иллюстрирует реализацию классом интерфейса IEnumerable 
// для использования его в операторе foreach
uses System.Collections;

type
  // Генератор чисел Фибоначчи
  FibGen = class(IEnumerable, IEnumerator)
  private
    a,b,n,i: integer;
  public
    constructor Create(n: integer);
    begin
      i := -1;
      a := 0;
      b := 1;
      Self.n := n;
    end;
    function Get_Current: Object;
    begin
      if i=0 then 
        Result := 1
      else Result := b;
    end;
    function GetEnumerator: IEnumerator;
    begin
      Result := Self;
    end;
    function MoveNext: boolean;
    begin
      i += 1;
      Result := i<n;
      if i=0 then exit;
      var c := a + b;
      a := b;
      b := c;
    end;
    property Current: Object read Get_Current;
    procedure Reset;
    begin
      i := -1;
    end;
  end;

var f: FibGen;

begin
  writeln('Числа Фибоначчи');
  f := new FibGen(25);
  foreach x: integer in f do
    write(x,' ');
end.