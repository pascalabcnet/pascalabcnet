uses
  System, System.Net, System.Threading.Tasks;

type
  Int = class
    N: int64;
    constructor(NN: int64);
    begin
      N := NN;
    end;
    
    function IsPrime: boolean;
    begin
      Result := True;
      for var i: int64 := 2 to round(sqrt(N)) do
        if N mod i = 0 then
        begin
          Result := False;
          exit;
        end;
    end;
  end;

type
  BTask = Task<boolean>;

begin
  var ss: array of int64 := (1265713689122381, 1265713689122461, 1265713689142451, 1265713689542501); // Все числа - простые
  var tb := new BTask[4];
  // Запуск 4 потоков из пула потоков
  // У каждого tb[i] свой Result!
  for var i := 0 to ss.Length - 1 do
    tb[i] := Task.Factory.StartNew((new Int(ss[i])).IsPrime);
  
  // Если поток не закончился, то обращение к tb[i].Result приостанавливает основную программу  
  for var i := 0 to tb.Length - 1 do
    writeln(tb[i].Result);
  // В эту точку мы попадем только после окончания всех четырех потоков  
  writeln(Milliseconds);  
  
  // Последовательное выполнение
  var t := Milliseconds;  
  for var i := 0 to ss.Length - 1 do
    writeln((new Int(ss[i])).IsPrime);
  writeln(Milliseconds - t);  
end.