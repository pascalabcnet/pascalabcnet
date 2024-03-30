unit SpythonHidden;

interface

uses PABCSystem;

function Floor(x : real) : integer;

function FloorDiv(x: real; y: real): integer;

function FloorMod(x: real; y: real): real;

procedure TestProcedure();

implementation

procedure TestProcedure();
begin
  println('test procedure called');
end;

function Floor(x : real) : integer := PABCSystem.Floor(x); 

function FloorDiv(x: real; y: real): integer := PABCSystem.Floor(x / y);

function FloorMod(x: real; y: real): real := x - PABCSystem.Floor(x / y) * y;

end.