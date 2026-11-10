unit ReexportBridge;

interface

uses ReexportFacade;

export ReexportFacade.PublicPoint as ChainedPoint,
       ReexportFacade.PublicCount as ChainedCount,
       ReexportFacade.Counter as ChainedCounter;

function MakePoint(x: integer): PublicPoint;
procedure AdvanceCounter(var delta: integer);

implementation

function MakePoint(x: integer): PublicPoint;
begin
  Result.X := x + PublicCount;
  BumpPoint(Result);
end;

procedure AdvanceCounter(var delta: integer);
begin
  Counter += delta;
  delta := Counter;
end;

end.
