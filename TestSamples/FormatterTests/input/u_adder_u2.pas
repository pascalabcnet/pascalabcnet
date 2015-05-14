unit u_adder_u2;

interface

//uses
//  System;

type
  adder<T> = template class
  public
    x, y: T;
    constructor(ax, ay: T);
    begin
      x := ax;
      y := ay;
    end;
    function GetResult: T;
    begin
      result := x + y;
    end;
    procedure WriteResult;
    begin
      WriteLn(x + y);
    end;
    function print_imp(x: integer): integer;
  end;

implementation

uses
  System;

var
  ix: integer := 0;
  
function adder<T>.print_imp(x: integer): integer;
begin
  Console.WriteLine('ppp');
  ix := ix + x;
  result := ix;
end;

end.