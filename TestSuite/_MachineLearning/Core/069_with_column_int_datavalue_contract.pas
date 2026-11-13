uses MLABC;

begin
  var df := DataFrame.FromCsvText('''
Количество,Цена
2,1.5
5,3.0
''');

  var withTotal := df.WithColumnInt('Сумма', row -> row['Количество'] + 1);
  Assert(withTotal.IntValues('Сумма').SequenceEqual([3, 6]));
  Assert(not df.HasColumn('Сумма'));

  var fromInt := df.WithColumnInt('Удвоено', row -> row.Int('Количество') * 2);
  Assert(fromInt.IntValues('Удвоено').SequenceEqual([4, 10]));

  var missing := df.WithColumnInt('Missing', [10, 20], [True, False]);
  var copied := missing.WithColumnInt('Copied', row -> row['Missing']);
  Assert(copied['Copied'].IsValid.SequenceEqual([True, False]));
  Assert(copied.Int(0, 'Copied') = 10);

  var rejected := false;
  try
    var wrongType := df.WithColumnInt('WrongType', row -> row['Цена']);
  except
    on e: Exception do rejected := true;
  end;
  Assert(rejected, 'A Float DataValue must not be silently converted to Int');
  Println('OK');
end.
