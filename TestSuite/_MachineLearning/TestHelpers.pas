unit TestHelpers;

interface

uses MLABC;

procedure Check(cond: boolean; msg: string);
procedure CheckRaises(action: procedure; msg: string);
procedure CheckSchemaMatchesColumns(df: DataFrame);
procedure CheckSchemaMatchesColumns(df: DataFrame; expectedCats: array of boolean);
procedure CheckProbabilityRowsSumToOne(m: Matrix; eps: real := 1e-9);

implementation

procedure Check(cond: boolean; msg: string);
begin
  if not cond then
    raise new Exception(msg);
end;

procedure CheckRaises(action: procedure; msg: string);
begin
  var raised := false;
  try
    action();
  except
    on e: Exception do
      raised := true;
  end;
  Check(raised, msg);
end;

procedure CheckSchemaMatchesColumns(df: DataFrame);
begin
  Check(df.Schema.ColumnCount = df.ColumnCount, 'Schema/column count mismatch');
  for var i := 0 to df.ColumnCount - 1 do
  begin
    Check(df.Schema.NameAt(i) = df.GetColumn(i).Info.Name, $'Name mismatch at {i}');
    Check(df.Schema.ColumnTypeAt(i) = df.GetColumn(i).Info.ColType, $'Type mismatch at {i}');
    Check(df.GetColumn(i).RowCount = df.RowCount, $'RowCount mismatch at column {i}');
  end;
end;

procedure CheckSchemaMatchesColumns(df: DataFrame; expectedCats: array of boolean);
begin
  CheckSchemaMatchesColumns(df);
  Check(expectedCats.Length = df.ColumnCount, 'Expected categorical flags length mismatch');
  for var i := 0 to df.ColumnCount - 1 do
    Check(df.Schema.IsCategoricalAt(i) = expectedCats[i], $'Categorical mismatch at {i}');
end;

procedure CheckProbabilityRowsSumToOne(m: Matrix; eps: real);
begin
  for var i := 0 to m.RowCount - 1 do
  begin
    var s := 0.0;
    for var j := 0 to m.ColCount - 1 do
      s += m[i, j];
    Check(Abs(s - 1.0) < eps, $'Probability row {i} must sum to 1');
  end;
end;

end.
