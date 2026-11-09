uses ReexportFacade;

begin
  if (F1(10) <> 11) or (F1('Pascal') <> 6) or
     (ReexportFacade.F1(20) <> 21) or (ReexportFacade.F1('abc') <> 3) then
    raise new Exception('Incorrect result from a re-exported function');

  var export := 0;
  FFF2(export);
  ReexportFacade.FFF2(export);
  if export <> 4 then
    raise new Exception('Incorrect result from a re-exported procedure');
end.
