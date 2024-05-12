unit Test;
interface

uses GraphABC;

const Dim = 5;
var Colors: array [1..Dim] of integer;

function RandomColor: integer;
procedure FillByRandomColor;

implementation
uses Timers;
var t := 3;
function RandomColor: integer;
begin
  Result := RGB(Random(255),Random(255),Random(255));
end;
procedure FillByRandomColor;
begin
  for i: integer := 1 to Dim do
    Colors[i] := RandomColor;
end;

initialization
  t := 4;
  FillByRandomColor;
end.