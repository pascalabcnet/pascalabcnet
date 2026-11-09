uses MLABC, MLExceptions;

procedure CheckConcatError(action: procedure);
begin
  var raised := false;
  try
    action();
  except
    on e: MLException do raised := true;
  end;
  Assert(raised, 'Invalid concatenation must raise MLException');
end;

begin
  var df := DataFrame.FromCsvText('Name,Surname,Quantity'#10'Anna,Smith,4')
    .WithColumnStr('Missing', ['Unknown'], [False]);
  var row := df.GetCursor;
  row.MoveNext;

  var suffix: string := '!';
  var right: string := row['Name'] + suffix;
  var left: string := suffix + row['Name'];
  Assert(right = 'Anna!');
  Assert(left = '!Anna');

  var computed := df.WithColumnStr('After', r -> r['Name'] + '!')
    .WithColumnStr('Before', r -> '!' + r['Name']);
  Assert(computed.Str(0, 'After') = 'Anna!');
  Assert(computed.Str(0, 'Before') = '!Anna');

  var both := row['Name'] + row['Surname'];
  Assert((both.ColType = ctStr) and (both.Str = 'AnnaSmith'));

  CheckConcatError(() -> begin var value := row['Missing'] + suffix; end);
  CheckConcatError(() -> begin var value := suffix + row['Missing']; end);
  CheckConcatError(() -> begin var value := row['Quantity'] + suffix; end);
  CheckConcatError(() -> begin var value := suffix + row['Quantity']; end);
  Println('OK');
end.
