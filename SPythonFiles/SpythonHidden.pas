{$HiddenIdents}
unit SpythonHidden;

interface

uses PABCSystem;

function !Floor(x : real) : integer;

function !FloorDiv(x: real; y: real): integer;

function !FloorMod(x: real; y: real): real;

type !UnknownType = class
end;

implementation

function !Floor(x : real) : integer := PABCSystem.Floor(x); 

function !FloorDiv(x: real; y: real): integer := PABCSystem.Floor(x / y);

function !FloorMod(x: real; y: real): real := x - PABCSystem.Floor(x / y) * y;

end.