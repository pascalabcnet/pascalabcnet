uses MLABC, MLExceptions;

procedure CheckDivisionError(action: procedure);
begin
  var raised := false;
  try
    action();
  except
    on e: MLException do raised := true;
  end;
  Assert(raised, 'Invalid division must raise MLException');
end;

begin
  var df := DataFrame.FromCsvText('A,B,Zero,Name'#10'4,2,0,Anna')
    .WithColumnInt('Missing', [4], [False]);
  var row := df.GetCursor;
  row.MoveNext;

  var intRight: real := row['A'] / 2;
  var intLeft: real := 1 / row['A'];
  var realRight: real := row['A'] / 2.0;
  var realLeft: real := 1.0 / row['A'];
  var both: real := row['B'] / row['A'];
  Assert((intRight = 2.0) and (intLeft = 0.25));
  Assert((realRight = 2.0) and (realLeft = 0.25));
  Assert(both = 0.5);

  var ratios := df.WithColumnFloat('Ratio', r -> r['B'] / r['A']);
  Assert(ratios.Float(0, 'Ratio') = 0.5);

  CheckDivisionError(() -> begin var value := row['A'] / row['Zero']; end);
  CheckDivisionError(() -> begin var value := row['A'] / 0; end);
  CheckDivisionError(() -> begin var value := 1 / row['Zero']; end);
  CheckDivisionError(() -> begin var value := row['Missing'] / row['B']; end);
  CheckDivisionError(() -> begin var value := row['A'] / row['Missing']; end);
  CheckDivisionError(() -> begin var value := row['Name'] / row['B']; end);
  CheckDivisionError(() -> begin var value := row['A'] / row['Name']; end);
  Println('OK');
end.
