uses ReexportFacade, ReexportBridge;

function ConstantKind(value: byte): integer := 1;
function ConstantKind(value: integer): integer := 2;

const ArraySize = ReexportFacade.PublicCount + 1;

begin
  if (F1(10) <> 11) or (F1('Pascal') <> 6) or
     (ReexportFacade.F1(20) <> 21) or (ReexportFacade.F1('abc') <> 3) then
    raise new Exception('Incorrect result from a re-exported function');

  var export := 0;
  FFF2(export);
  ReexportFacade.FFF2(export);
  if export <> 4 then
    raise new Exception('Incorrect result from a re-exported procedure');

  var point: ReexportFacade.PublicPoint;
  var synonym: PublicAlias := MakePoint(2);
  var chained: ChainedPoint := synonym;
  point := chained;
  BumpPoint(point); // requires the original type, not a structurally similar record
  assert(point.X = 11);

  var box := new PublicBox<integer>;
  box.Value := 23;
  assert(BoxValue(box) = 23);

  var values: array[1..ArraySize] of integer;
  values[ArraySize] := UnchangedConstant;
  assert(values[8] = 11);
  assert(ConstantKind(PublicCount) = 1);
  assert(ConstantKind(ChainedCount) = 1);
  assert(PublicCount = 7);

  assert(Counter = 10);
  ReexportFacade.Counter := 20;
  assert(ReadOriginCounter = 20);
  FFF2(ChainedCounter); // writes the same storage through a var parameter
  assert(ReadOriginCounter = 22);
  var delta := 3;
  AdvanceCounter(delta); // modifies the original variable from another compiled unit
  assert((delta = 25) and (Counter = 25) and (ChainedCounter = 25));
end.
