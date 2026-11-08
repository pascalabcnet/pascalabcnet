{$DisableStandardUnits}

uses GenericInitializationUnit;

begin
  var box := new UnitBox<string>;
  if (box.Value <> '') or (box.ReadLocal <> '') then
    raise new System.Exception('generic initialization from PCU');
end.
