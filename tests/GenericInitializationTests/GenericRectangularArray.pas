type
  StringVector = array of string;
  InnerRecord = record
    Text: string;
  end;
  SampleRecord = record
    Text: string;
    Number: integer;
    Inner: InnerRecord;
  end;

var sizeTrace := 0;

function Size(n: integer): integer;
begin
  sizeTrace := 10 * sizeTrace + n;
  Result := n;
end;

function AllocateMatrix<T>(n, m: integer): array[,] of T := new T[n,m];
function AllocateCube<T>(n, m, k: integer): array[,,] of T := new T[n,m,k];
function AllocateExplicit<T>(item: T): array[,] of T :=
  new T[2,2]((item, default(T)), (default(T), item));

procedure Check(condition: boolean; message: string);
begin
  if not condition then
    raise new System.Exception(message);
end;

begin
  var strings := AllocateMatrix&<string>(Size(2), Size(3));
  Check((sizeTrace = 23) and (strings.GetLength(0) = 2) and (strings.GetLength(1) = 3),
    'dimensions evaluated once in order');
  for var i := 0 to 1 do
    for var j := 0 to 2 do
      Check(strings[i,j] = '', 'each matrix string is initialized');

  var records := AllocateCube&<SampleRecord>(2, 2, 3);
  for var i := 0 to 1 do
    for var j := 0 to 1 do
      for var k := 0 to 2 do
      begin
        Check(records[i,j,k].Text = '', 'each cube record is initialized');
        Check(records[i,j,k].Inner.Text = '', 'nested record is initialized');
        Check(records[i,j,k].Number = 0, 'ordinary record fields retain their default');
      end;

  var zeroRows := AllocateMatrix&<string>(0, 3);
  var zeroColumns := AllocateMatrix&<SampleRecord>(2, 0);
  var zeroMiddle := AllocateCube&<SampleRecord>(2, 0, 3);
  Check((zeroRows.Length = 0) and (zeroColumns.Length = 0) and (zeroMiddle.Length = 0),
    'a zero dimension leaves the array empty');

  var integers := AllocateMatrix&<integer>(2, 3);
  Check(integers[1,2] = 0, 'integers are unchanged');
  var nested := AllocateMatrix&<StringVector>(1, 2);
  Check(System.Object.ReferenceEquals(nested[0,1], nil), 'inner arrays are not allocated');

  var explicitStrings := AllocateExplicit('provided');
  Check((explicitStrings[0,0] = 'provided') and (explicitStrings[1,1] = 'provided'),
    'explicit string values are preserved');
  Check(System.Object.ReferenceEquals(explicitStrings[0,1], nil), 'explicit default string is preserved');

  var item: SampleRecord;
  item.Text := 'record';
  item.Inner.Text := 'inner';
  item.Number := 17;
  var explicitRecords := AllocateExplicit(item);
  Check((explicitRecords[1,1].Text = 'record') and (explicitRecords[1,1].Inner.Text = 'inner') and
    (explicitRecords[1,1].Number = 17), 'explicit record values are preserved');
  Check(System.Object.ReferenceEquals(explicitRecords[0,1].Text, nil), 'explicit default record is preserved');
end.
