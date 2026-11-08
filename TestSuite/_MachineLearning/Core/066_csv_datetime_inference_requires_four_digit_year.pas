uses MLABC, DataFrameABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := DataFrame.FromCsvText('''
specialty,date
01.01.01,01.01.2024
02.02.02,02.02.2024
''');

  Check(df.GetColumnType('specialty') = ctStr,
    'A value with a two-digit year must remain a string');
  Check(df.GetColumnType('date') = ctDateTime,
    'A date with a four-digit year must be inferred as DateTime');

  var types := new Dictionary<string, ColumnType>;
  types['date_time'] := ctDateTime;
  var explicitDf := DataFrame.FromCsvText('''
date_time
15.01.2024 12:30
''', columnTypes := types);

  Check(explicitDf.GetColumnType('date_time') = ctDateTime,
    'An explicitly declared DateTime column must accept a date with time');
end.
