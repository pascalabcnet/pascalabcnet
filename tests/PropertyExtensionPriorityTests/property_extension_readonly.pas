// TEST-REGRESSION: selecting a property must preserve readonly diagnostics.
{$reference PropertyExtensionFixture.dll}
uses PropertyExtensionFixture;
begin
  var gauge := new Gauge;
  gauge.ReadOnly := 1;
end.
