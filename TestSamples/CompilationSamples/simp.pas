
uses System;

var ii : integer;

const max = 100;
      max2 = max+10;

// Генерация больших простых чисел
var i, j, count: integer;
    f: boolean;
    beg: integer;

begin
  count:=0; writeln(max2);
  beg:=1000000000;
  i := beg;
  //for i := 1000000000 to 1000001000 do
  while i<beg+1000 do
  begin
    f:=true;
    j:=2;
    while f and (j<=Convert.ToInt32(Math.Round(Math.Sqrt(Convert.ToDouble(i))))) do
      if i mod j = 0 then f:=false
        else j:=j+1;
    if f then
    begin
      Console.Write(i);
	Console.Write(' ');   	
      count := count+1;
      if count mod 5 = 0 then Console.WriteLine;	
    end;
   i := i+1;
  end;
end.
