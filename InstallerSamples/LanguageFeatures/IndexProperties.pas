// Индексные свойства. Массив цветных квадратов 
uses GraphABC;

const 
  sz = 50;
  dim = 10;
  delay = 500;

type 
  /// 
  VisualArray = class
  private
    a: array of Color;
    procedure SetItem(i: integer; x: Color); // Процедура, устанавливающая цвет i-того квадрата
    begin
      if (i<0) or (i>=a.Length) then
        raise new System.ArgumentException('Выход за границы изменения индекса: '+IntToStr(i));
      a[i] := x;
      Draw(i);
    end;
    function GetItem(i: integer): Color; // Функция, возвращающая цвет i-того квадрата
    begin
      if (i<0) or (i>=a.Length) then
        raise new System.ArgumentException('Выход за границы изменения индекса: '+IntToStr(i));
      Result := a[i];
    end;
  public
    constructor (n: integer);
    begin
      a := ArrFill(n,Color.White);
      Draw;
    end;
    /// Рисует i-тый квадрат
    procedure Draw(i: integer);
    begin
      Brush.Color := a[i];
      Rectangle(sz+sz*i,sz,sz+sz*(i+1)+1,sz+sz);
    end;
    /// Рисует массив цветных квадратов
    procedure Draw;
    begin
      for var i:=0 to a.Length-1 do
        Draw(i);
    end;
    /// Индексное свойство по умолчанию
    property Item[i: integer]: Color read GetItem write SetItem; default;
  end;

var arr: VisualArray;

begin
  Window.Title := 'Иллюстрация индексных свойств';
  arr := new VisualArray(dim);
  Window.SetSize(sz*(dim+2),3*sz);
  arr[0] := clGreen; // arr.Items[0] = arr[0] поскольку свойство Items является свойством по умолчанию
  Sleep(delay);
  arr[1] := clBlack;
  Sleep(delay);
  arr[2] := clYellow;
  Sleep(delay);
  arr[3] := clGray;
  Sleep(delay);
  arr[4] := clRed;
  Sleep(delay);
  arr[5] := clMagenta;
  Sleep(delay);
  arr[6] := clBrown;
  Sleep(delay);
  arr[7] := clMoneyGreen;
  Sleep(delay);
  arr[8] := clOlive;
  Sleep(delay);
  arr[9] := clLinen;
end.