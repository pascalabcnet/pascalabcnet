uses MLABC, MLExceptions;

procedure CheckTypeError(action: procedure);
begin
  var raised := false;
  try
    action();
  except
    on e: MLException do raised := true;
  end;
  Assert(raised, 'A valid value of the wrong type must raise MLException');
end;

begin
  var dt := new System.DateTime(2024, 9, 1);
  var mask := [True, False];
  var df := DataFrame.FromCsvText('''
Quantity
3
7
''')
    .WithColumnStr('Name', ['Anna', 'Boris'], mask)
    .WithColumnBool('Active', [False, True], mask)
    .WithColumnDateTime('Date', [dt, dt.AddDays(1)], mask)
    .WithColumnFloat('Price', [1.5, 2.5], mask);

  var copies := df.WithColumnStr('NameCopy', row -> row['Name'])
    .WithColumnBool('ActiveCopy', row -> row['Active'])
    .WithColumnDateTime('DateCopy', row -> row['Date'])
    .WithColumnFloat('PriceCopy', row -> row['Price']);
  foreach var name in ['NameCopy', 'ActiveCopy', 'DateCopy', 'PriceCopy'] do
    Assert(copies[name].IsValid.SequenceEqual(mask));
  Assert(copies.Str(0, 'NameCopy') = 'Anna');
  Assert(not copies.Bool(0, 'ActiveCopy'));
  Assert(copies.DateTime(0, 'DateCopy') = dt);
  Assert(copies.Float(0, 'PriceCopy') = 1.5);
  Assert(not df.HasColumn('NameCopy'));

  var computed := df.Head(1)
    .WithColumnStr('Exclaim', row -> row['Name'] + '!')
    .WithColumnStr('Lower', row -> row.Str('Name').ToLowerInvariant())
    .WithColumnBool('Large', row -> row['Quantity'] >= 3)
    .WithColumnDateTime('Tomorrow', row -> row.DateTime('Date').AddDays(1))
    .WithColumnFloat('AsFloat', row -> row['Quantity']);
  Assert(computed.Str(0, 'Exclaim') = 'Anna!');
  Assert(computed.Str(0, 'Lower') = 'anna');
  Assert(computed.Bool(0, 'Large'));
  Assert(computed.DateTime(0, 'Tomorrow') = dt.AddDays(1));
  Assert(computed.Float(0, 'AsFloat') = 3.0);

  var arithmetic := df.WithColumnInt('Plus', row -> row['Quantity'] + 1)
    .WithColumnInt('PlusLeft', row -> 1 + row['Quantity'])
    .WithColumnInt('Times', row -> row['Quantity'] * 2)
    .WithColumnInt('TimesLeft', row -> 2 * row['Quantity'])
    .WithColumnFloat('TimesFloat', row -> row['Quantity'] * 0.5);
  Assert(arithmetic.IntValues('Plus').SequenceEqual([4, 8]));
  Assert(arithmetic.IntValues('PlusLeft').SequenceEqual([4, 8]));
  Assert(arithmetic.IntValues('Times').SequenceEqual([6, 14]));
  Assert(arithmetic.IntValues('TimesLeft').SequenceEqual([6, 14]));
  Assert(arithmetic.FloatValues('TimesFloat').SequenceEqual([1.5, 3.5]));
  var scaled := df.Head(1).WithColumnFloat('ScaledPrice', row -> row['Price'] * 2);
  Assert(scaled.Float(0, 'ScaledPrice') = 3.0);

  CheckTypeError(() -> begin var res := df.WithColumnStr('Wrong', row -> row['Quantity']); end);
  CheckTypeError(() -> begin var res := df.WithColumnBool('Wrong', row -> row['Quantity']); end);
  CheckTypeError(() -> begin var res := df.WithColumnDateTime('Wrong', row -> row['Quantity']); end);
  CheckTypeError(() -> begin var res := df.WithColumnFloat('Wrong', row -> row['Name']); end);
  CheckTypeError(() -> begin var res := df.WithColumnInt('Wrong', row -> row['Price'] * 2); end);
  Println('OK');
end.
