// Демонстрация создания простого класса стека на базе массива
type
  Stack<T> = class
  private
    a: array of T;
    last: integer;
  public  
    constructor Create(sz: integer);
    begin
      SetLength(a,sz);
      last := 0;
    end;
    constructor Create;
    begin
      Create(100);
    end;
    procedure push(i: T);
    begin
      a[last] := i;
      Inc(last);
    end;
    function pop: T;
    begin
      Dec(last);
      pop := a[last];
    end;
    function top: T;
    begin
      top := a[last-1];
    end;
    function empty: boolean;
    begin
      Result := (last=0);
    end;
    function ToString: string; override;
    begin
      Result := '';
      for var i:=0 to last-1 do
        Result += a[i]+' ';
    end;
  end;
  
var s: Stack<integer>;

begin
  s := new Stack<integer>;
  s.push(7);
  s.push(2);
  s.push(5);
  s.push(4);
  writeln(s);
  while not s.empty do
    write(s.pop,' ');
end.


