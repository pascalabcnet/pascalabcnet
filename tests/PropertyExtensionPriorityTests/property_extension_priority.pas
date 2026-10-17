// TEST-REGRESSION: assignment must prefer a property over an extension method.
{$reference PropertyExtensionFixture.dll}
uses PropertyExtensionFixture;

begin
  var gauge := new Gauge;
  gauge.Width := 60;
  assert(gauge.Width.Value = 60);
  gauge.Label := 'property';
  assert(gauge.Label = 'property');
  gauge.Width(20);
  assert(gauge.Width.Value = 20);
  gauge.Width(15, true);
  assert(gauge.Width.Value = 30);
  gauge.Label('method');
  assert(gauge.Label = 'method');
  assert(gauge.ReadOnly = 42);
  gauge.ReadOnly(123);
  gauge.OnlyMethod(1);

  var derived := new DerivedGauge;
  derived.Width := 17;
  assert(derived.Width.Value = 17);
  derived.Width(23);
  assert(derived.Width.Value = 23);
end.
