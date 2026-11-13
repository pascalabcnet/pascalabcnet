uses ReexportOrigin, ReexportFacade;

begin
  if ReexportOrigin.Identity&<integer>(73) <> 73 then
    raise new Exception('Incorrect result from a qualified generic function');
  if ReexportFacade.Same&<integer>(73) <> 73 then
    raise new Exception('Incorrect result from a qualified generic re-export');
  if ReexportFacade.Same&<string>('Pascal') <> 'Pascal' then
    raise new Exception('Incorrect explicit type argument for a generic re-export');
  if (ReexportFacade.Same(73) <> 74) or (ReexportFacade.Same('Pascal') <> 'Pascal') then
    raise new Exception('Incorrect overload selection without an explicit type argument');
  if ReexportOrigin.Identity('Pascal') <> 'Pascal' then
    raise new Exception('Incorrect result from a qualified generic function with inferred type');
  if ReexportFacade.SelectItem(7, 'first', 'second') <> 'first' then
    raise new Exception('Incorrect result from a re-exported generic function with constraints');
end.
