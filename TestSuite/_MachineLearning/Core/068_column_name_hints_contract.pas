uses DataFrameABC, DataFrameABCCore;

procedure Check(condition: boolean; message: string);
begin
  if not condition then raise new Exception(message);
end;

function Missing(schema: DataFrameSchema; name: string): string;
begin
  Result := '';
  try
    var index := schema.IndexOf(name);
  except
    on e: System.ArgumentException do Result := e.Message;
  end;
  Check(Result.Length > 0, 'Missing column must raise ArgumentException');
  Check(not Result.Contains(#10) and not Result.Contains(#13), 'Message must fit on one line');
end;

begin
  var schema := new DataFrameSchema(['City', 'Population', 'Area', 'Население'],
    [ctStr, ctInt, ctFloat, ctInt]);
  Check(schema.IndexOf('Population') = 1, 'Exact lookup must succeed');
  Check(not schema.HasColumn('population'), 'HasColumn must remain case-sensitive');
  Check(not schema.HasColumn(' Population '), 'HasColumn must preserve spaces');
  var message := Missing(schema, ' population ');
  Check(message.Contains('"Population"'), 'Case and spaces must suggest the original name');
  message := Missing(schema, 'populaton');
  Check(message.Contains('"Population"'), 'Insertion typo must be suggested');
  message := Missing(schema, 'Populasion');
  Check(message.Contains('"Population"'), 'Substitution typo must be suggested');
  message := Missing(schema, 'Populatioon');
  Check(message.Contains('"Population"'), 'Extra character must be handled');
  message := Missing(schema, ' население ');
  Check(message.Contains('"Население"'), 'Cyrillic case and spaces must be handled');
  message := Missing(schema, 'people');
  Check(message.Contains('City, Population, Area, Население'), 'Fallback must list available columns');

  var ambiguous := new DataFrameSchema(['City', 'CITY', ' City '], [ctInt, ctInt, ctInt]);
  Check(ambiguous.IndexOf('CITY') = 1, 'Case-distinct names must remain valid');
  Check(ambiguous.IndexOf(' City ') = 2, 'Names with spaces must remain valid');
  message := Missing(ambiguous, 'city');
  Check(message.Contains('"City"') and message.Contains('"CITY"'), 'Must suggest all normalized matches');

  var ranked := new DataFrameSchema(['PopulationX', 'Populaton', 'PopulationY', 'PopulationZ'],
    [ctInt, ctInt, ctInt, ctInt]);
  message := Missing(ranked, 'population');
  Check(message.Contains('"PopulationX"') and message.Contains('"Populaton"') and
    message.Contains('"PopulationY"') and not message.Contains('"PopulationZ"'),
    'Suggestions must be limited to three with stable ties');

  var names := new string[15];
  var types := new ColumnType[15];
  for var i := 0 to 14 do names[i] := 'field_' + i.ToString;
  var wide := new DataFrameSchema(names, types);
  message := Missing(wide, 'zzzzzzzzzzzzzzzzzzzz');
  Check(message.Contains('field_9') and not message.Contains('field_10'), 'Fallback must be limited to ten');
  Check(message.Contains('5') and message.Contains('df.Schema.ColumnNames'), 'Must explain the truncated list');
  var empty := new DataFrameSchema(new string[0], new ColumnType[0]);
  message := Missing(empty, 'anything');
  message := Missing(schema, '');

  var df := DataFrame.FromCsvText('''
City,Population
Moscow,100
''');
  var raised := false;
  try
    var col := df['population'];
  except
    on e: System.ArgumentException do
      raised := e.Message.Contains('"Population"');
  end;
  Check(raised, 'DataFrame indexer must include suggestions');
  var cursor := df.GetCursor;
  cursor.MoveNext;
  raised := false;
  try
    var value := cursor.Int(' populaton ');
  except
    on e: System.ArgumentException do
      raised := e.Message.Contains('"Population"');
  end;
  Check(raised, 'Cursor must include suggestions');
  Check(df['Population'].IntValues[0] = 100, 'Failed lookups must not change data');
  Check(df.Schema.ColumnNames.SequenceEqual(['City', 'Population']), 'Names must stay unchanged');
  Println('OK');
end.
