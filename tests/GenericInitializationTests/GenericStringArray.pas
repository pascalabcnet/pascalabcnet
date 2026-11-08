type
  StringVector = array of string;
  NestedRecord = record
    Value: string;
  end;
  SampleRecord = record
    Value: string;
    Number: integer;
    Nested: NestedRecord;
  end;

var sizeCalls := 0;

function Size: integer;
begin
  sizeCalls += 1;
  Result := 3;
end;

function Allocate<T>(n: integer): array of T := new T[n];
function AllocateExplicit<T>(value: T): array of T := new T[2](value, default(T));

procedure Check(condition: boolean; message: string);
begin
  if not condition then
    raise new System.Exception(message);
end;

begin
  var strings := Allocate&<string>(Size);
  Check((sizeCalls = 1) and (strings.Length = 3), 'array size is evaluated once');
  for var i := 0 to strings.Length - 1 do
    Check(strings[i] = '', 'each string element is initialized');
  var empty := Allocate&<string>(0);
  Check(empty.Length = 0, 'empty string array');

  var integers := Allocate&<integer>(2);
  Check((integers[0] = 0) and (integers[1] = 0), 'integer elements are unchanged');
  var records := Allocate&<SampleRecord>(2);
  var concreteRecords := new SampleRecord[2];
  for var i := 0 to records.Length - 1 do
  begin
    Check(records[i].Value = '', 'record initializer runs for each element');
    Check(records[i].Nested.Value = '', 'nested record initializer runs');
    Check(records[i].Number = concreteRecords[i].Number, 'record keeps ordinary default values');
  end;
  var emptyRecords := Allocate&<SampleRecord>(0);
  Check(emptyRecords.Length = 0, 'empty record array');
  var nested := Allocate&<StringVector>(1);
  Check(System.Object.ReferenceEquals(nested[0], nil), 'inner array is not allocated');

  var explicitValues := AllocateExplicit('provided');
  Check(explicitValues[0] = 'provided', 'explicit string is preserved');
  Check(System.Object.ReferenceEquals(explicitValues[1], nil), 'explicit default is preserved');
  var providedRecord: SampleRecord;
  providedRecord.Value := 'provided';
  providedRecord.Nested.Value := 'nested';
  providedRecord.Number := 17;
  var explicitRecords := AllocateExplicit(providedRecord);
  Check((explicitRecords[0].Value = 'provided') and (explicitRecords[0].Nested.Value = 'nested') and
    (explicitRecords[0].Number = 17), 'explicit record is preserved');
  Check(System.Object.ReferenceEquals(explicitRecords[1].Value, nil), 'explicit default record is preserved');
end.
