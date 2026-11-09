uses MLABC;

begin
  var df := DataFrame.FromCsvText('Quantity,Price,Name'#10'4,2.0,Anna'#10'8,8.0,Boris');

  var computed := df.WithColumnInt('Adjusted', row -> row['Quantity'] + 1 - 2)
    .WithColumnFloat('Ratio', row -> (row['Quantity'] + 1) / row['Price'])
    .WithColumnStr('Wrapped', row -> '(' + row['Name'] + ')');
  Assert(computed.IntValues('Adjusted').SequenceEqual([3, 7]));
  Assert(computed.FloatValues('Ratio').SequenceEqual([2.5, 1.125]));
  Assert(computed.StrValues('Wrapped').SequenceEqual(['(Anna)', '(Boris)']));

  var row := df.GetCursor;
  row.MoveNext;
  var ratio: real := (row['Quantity'] + 1) / row['Price'];
  var mixed: real := ratio + row['Quantity'] - 0.5;
  var wrapped: string := '(' + row['Name'] + ')';
  Assert(ratio = 2.5);
  Assert(mixed = 6.0);
  Assert(wrapped = '(Anna)');

  Assert(df.Sum(r -> r['Price'] / r['Quantity']) = 1.5);
  Assert(df.Sum(r -> (r['Quantity'] + 1) / r['Price']) = 3.625);
  Assert(df.Sum(r -> r['Quantity'] + 1 - 2) = 10);
  Println('OK');
end.
