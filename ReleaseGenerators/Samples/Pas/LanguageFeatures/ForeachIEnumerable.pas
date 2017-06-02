// Пример иллюстрирует реализацию классом интерфейса IEnumerable 
// для использования его в операторе foreach
type
  // Генератор чисел Фибоначчи
  FibGen = class(IEnumerable<integer>, IEnumerator<integer>)
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
    function Get_Current: integer;
    begin
      if i=0 then 
        Result := 1
      else Result := b;
    end;
    function System.Collections.IEnumerator.Get_Current: object := Get_Current;
    function GetEnumerator: IEnumerator<integer> := Self;
    function System.Collections.IEnumerable.GetEnumerator: System.Collections.IEnumerator := Self;
    function MoveNext: boolean;
    begin
      i += 1;
      Result := i<n;
      if i=0 then exit;
      (a,b) := (b,a+b);
    end;
    property Current: integer read Get_Current;
    procedure Reset;
    begin
    end;
    procedure Dispose;
    begin
    end;
  end;

begin
  writeln('Числа Фибоначчи');
  var f := new FibGen(25);
  foreach var x in f do
    Print(x);
end.