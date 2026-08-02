unit u_foreach_nested_enum_generic1;

interface

type
  // NET10-TESTFIX regression: EnumBuilder nested in an array and a generic type.
  AggregationKind = (akCount, akSum, akMean, akStd, akMin, akMax);

implementation

procedure Aggregate(map: Dictionary<string, array of AggregationKind>);
begin
  foreach var kvp in map do
  begin
  end;
end;

end.
