// TEST-REGRESSION: without a property, a method is still not assignable.
{$reference PropertyExtensionFixture.dll}
uses PropertyExtensionFixture;
begin
  var gauge := new Gauge;
  gauge.OnlyMethod := 1;
end.
