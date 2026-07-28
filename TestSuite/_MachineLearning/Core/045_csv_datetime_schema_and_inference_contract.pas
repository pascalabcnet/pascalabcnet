uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var textIso := '''
id,created_at,name
1,2024-01-15,Alice
2,2024-01-16T12:30:00,Bob
''';

  var textRu := '''
id,created_at,name
1,15.01.2024,Alice
2,16.01.2024 12:30:00,Bob
''';

  var columnTypes := new Dictionary<string, ColumnType>;
  columnTypes['created_at'] := ColumnType.ctDateTime;

  var dfSchema := DataFrame.FromCsvText(
    textIso,
    columnTypes := columnTypes
  );

  Check(dfSchema.GetColumnType('created_at') = ColumnType.ctDateTime, 'Schema DateTime type expected');
  Check(dfSchema.DateTime('created_at')[0] = new System.DateTime(2024, 1, 15), 'Schema DateTime value mismatch');

  var dfInfer := DataFrame.FromCsvText(
    textIso,
    inferTypes := true
  );

  Check(dfInfer.GetColumnType('created_at') = ColumnType.ctDateTime, 'Inferred DateTime type expected');
  Check(dfInfer.DateTime('created_at')[1] = new System.DateTime(2024, 1, 16, 12, 30, 0), 'Inferred DateTime value mismatch');

  var dfRu := DataFrame.FromCsvText(
    textRu,
    inferTypes := true
  );

  Check(dfRu.GetColumnType('created_at') = ColumnType.ctDateTime, 'Russian DateTime inference expected');
  Check(dfRu.DateTime('created_at')[0] = new System.DateTime(2024, 1, 15), 'Russian DateTime date mismatch');
  Check(dfRu.DateTime('created_at')[1] = new System.DateTime(2024, 1, 16, 12, 30, 0), 'Russian DateTime value mismatch');

  var tmp := System.IO.Path.Combine(
    System.IO.Path.GetTempPath,
    'ml_datetime_' + System.Guid.NewGuid.ToString + '.csv'
  );

  dfInfer.ToCsv(tmp);
  var dfRoundTrip := DataFrame.FromCsv(tmp);

  Check(dfRoundTrip.GetColumnType('created_at') = ColumnType.ctDateTime, 'Round-trip DateTime type expected');
  Check(dfRoundTrip.DateTime('created_at')[1] = dfInfer.DateTime('created_at')[1], 'Round-trip DateTime value mismatch');
  CheckSchemaMatchesColumns(dfRoundTrip);

  if System.IO.File.Exists(tmp) then
    System.IO.File.Delete(tmp);
end.
