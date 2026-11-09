uses MLABC, MLExceptions;

procedure CheckArithmeticError(action: procedure);
begin
  var raised := false;
  try
    action();
  except
    on e: MLException do raised := true;
  end;
  Assert(raised, 'Invalid arithmetic must raise MLException');
end;

begin
  var df := DataFrame.FromCsvText('Quantity,Price,Name'#10'4,1.5,Anna')
    .WithColumnInt('Missing', [0], [False]);
  var row := df.GetCursor;
  row.MoveNext;

  var plusRight: real := row['Quantity'] + 2.0;
  var plusLeft: real := 2.0 + row['Quantity'];
  var minusRight: real := row['Quantity'] - 2.0;
  var minusLeft: real := 2.0 - row['Quantity'];
  Assert((plusRight = 6.0) and (plusLeft = 6.0));
  Assert((minusRight = 2.0) and (minusLeft = -2.0));

  var ints := df.WithColumnInt('PlusRight', r -> r['Quantity'] + 1)
    .WithColumnInt('PlusLeft', r -> 1 + r['Quantity'])
    .WithColumnInt('MinusRight', r -> r['Quantity'] - 1)
    .WithColumnInt('MinusLeft', r -> 1 - r['Quantity']);
  Assert((ints.Int(0, 'PlusRight') = 5) and (ints.Int(0, 'PlusLeft') = 5));
  Assert((ints.Int(0, 'MinusRight') = 3) and (ints.Int(0, 'MinusLeft') = -3));

  var mixed := row['Price'] + 1;
  var reversed := 1 - row['Price'];
  Assert((mixed.ColType = ctFloat) and (mixed.Float = 2.5));
  Assert((reversed.ColType = ctFloat) and (reversed.Float = -0.5));
  var twoCells := row['Quantity'] + row['Price'];
  Assert((twoCells.ColType = ctFloat) and (twoCells.Float = 5.5));

  var floats := df.WithColumnFloat('Adjusted', r -> r['Price'] + 2.0);
  Assert(floats.Float(0, 'Adjusted') = 3.5);
  var strings := df.WithColumnStr('Greeting', r -> r['Name'] + '!');
  Assert(strings.Str(0, 'Greeting') = 'Anna!');

  CheckArithmeticError(() -> begin var value := row['Missing'] + 2.0; end);
  CheckArithmeticError(() -> begin var value := 2.0 - row['Missing']; end);
  CheckArithmeticError(() -> begin var value := row['Name'] - 2.0; end);
  CheckArithmeticError(() -> begin var value := 2.0 + row['Name']; end);
  Println('OK');
end.
