uses MLABC;

begin
  var df := DataFrame.FromCsvText('''
id
1
2
3
4
''').WithColumnBool('Активен', [True, False, True, True], [True, True, False, True]);

  var active := df.Filter(row -> row['Активен']);
  Assert(active.IntValues('id').SequenceEqual([1, 4]));
  Assert(df.RowCount = 4);

  var byComparison := df.Filter(row -> row['id'] >= 3);
  Assert(byComparison.IntValues('id').SequenceEqual([3, 4]));
  var byBool := df.Filter(row -> row.Int('id') = 2);
  Assert(byBool.IntValues('id').SequenceEqual([2]));

  var rejected := false;
  try
    var wrongType := df.Filter(row -> row['id']);
  except
    on e: Exception do rejected := true;
  end;
  Assert(rejected, 'A valid non-boolean DataValue cannot be a predicate');
  Println('OK');
end.
