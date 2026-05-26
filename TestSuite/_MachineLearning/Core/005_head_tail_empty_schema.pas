uses MLABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

procedure CheckSameSchema(a, b: DataFrame; prefix: string);
begin
  Check(a.Schema.ColumnCount = b.Schema.ColumnCount, prefix + ': column count mismatch');
  for var i := 0 to a.Schema.ColumnCount - 1 do
  begin
    Check(a.Schema.NameAt(i) = b.Schema.NameAt(i), prefix + $': name mismatch at {i}');
    Check(a.Schema.ColumnTypeAt(i) = b.Schema.ColumnTypeAt(i), prefix + $': type mismatch at {i}');
    Check(a.Schema.IsCategoricalAt(i) = b.Schema.IsCategoricalAt(i), prefix + $': categorical mismatch at {i}');
  end;
end;

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('Msk', 'Spb', 'Kzn'));
  df.AddIntColumn('Age', Arr(20, 30, 40));
  df := df.SetCategorical(['City']);

  var h := df.Head(0);
  var t := df.Tail(0);

  Check(h.RowCount = 0, 'Head(0) must have 0 rows');
  Check(t.RowCount = 0, 'Tail(0) must have 0 rows');
  CheckSameSchema(df, h, 'Head(0)');
  CheckSameSchema(df, t, 'Tail(0)');
end.
