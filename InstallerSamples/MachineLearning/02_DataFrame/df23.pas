uses MLABC;

begin
  var text := '''
id,created_at,name
1,2024-01-15,Alice
2,2024-01-16 12:30:00,Bob
''';
  var df1 := DataFrame.FromCsvText(
    text,
    columnTypes := Dict('created_at' to ColumnType.ctDateTime)
  );

  Println('Явная схема:');
  df1.Print;
  Println(df1.DateTimeValues('created_at')[0].Year);

  var df2 := DataFrame.FromCsvText(text);

  Println;
  Println('Автоопределение:');
  df2.Print(dateTimeFormat := 'dd.MM.yy');
  Println(df2.GetColumnType('created_at'));
end.
