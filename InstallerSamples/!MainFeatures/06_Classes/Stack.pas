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
    procedure Push(i: T);
    begin
      a[last] := i;
      Inc(last);
    end;
    function Pop: T;
    begin
      Dec(last);
      Result := a[last];
    end;
    function Top: T;
    begin
      Result := a[last-1];
    end;
    function Empty: boolean;
    begin
      Result := last=0;
    end;
    function ToString: string; override;
    begin
      Result := '';
      for var i:=0 to last-1 do
        Result += a[i]+' ';
    end;
  end;
  
begin
  var s := new Stack<integer>;
  s.Push(7);
  s.Push(2);
  s.Push(5);
  s.Push(4);
  Println(s);
  while not s.Empty do
    Print(s.Pop);
end.


