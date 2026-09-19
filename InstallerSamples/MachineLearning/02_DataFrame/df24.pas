uses MLABC;

begin
  var text := '''
id,created_at,name
1,15.01.2024,Alice
2,16.01.2024 12:30:00,Bob
''';

  var df := DataFrame.FromCsvText(text);

  df.Print;
  Println;
  Println(df.GetColumnType('created_at'));
  Println(df.DateTimeValues('created_at')[0].Year);
  Println(df.DateTimeValues('created_at')[1].Hour);
end.
