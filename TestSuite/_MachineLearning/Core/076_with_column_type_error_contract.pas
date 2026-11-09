uses MLABC, MLExceptions;

procedure CheckTypeError(action: procedure; name, expected, actual: string);
begin
  var message := '';
  try
    action();
  except
    on e: MLException do message := e.Message;
  end;
  Assert(message.Contains('"' + name + '"'), 'Expected destination column name: ' + message);
  var expectedPos := message.IndexOf(expected);
  var actualPos := message.IndexOf(actual);
  Assert((expectedPos >= 0) and (actualPos > expectedPos),
    'Expected the requested type followed by the actual type: ' + message);
end;

begin
  var df := DataFrame.FromCsvText('Quantity,Name,Active'#10'4,Anna,true')
    .WithColumnFloat('Missing', [2.1], [False]);

  CheckTypeError(() -> begin var r := df.WithColumnInt('Total', row -> row['Quantity'] * 2.1); end,
    'Total', 'Int', 'Float');
  CheckTypeError(() -> begin var r := df.WithColumnFloat('Amount', row -> row['Name']); end,
    'Amount', 'Float', 'Str');
  CheckTypeError(() -> begin var r := df.WithColumnStr('Label', row -> row['Quantity']); end,
    'Label', 'Str', 'Int');
  CheckTypeError(() -> begin var r := df.WithColumnBool('Flag', row -> row['Name']); end,
    'Flag', 'Bool', 'Str');
  CheckTypeError(() -> begin var r := df.WithColumnDateTime('Date', row -> row['Active']); end,
    'Date', 'DateTime', 'Bool');

  var converted := df.WithColumnFloat('Converted', row -> row['Quantity'])
    .WithColumnInt('CopiedMissing', row -> row['Missing']);
  Assert(converted.Float(0, 'Converted') = 4.0);
  Assert(not converted['CopiedMissing'].IsValid[0]);
  Println('OK');
end.
