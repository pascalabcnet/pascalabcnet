uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('A', 'B'));
  df.AddIntColumn('Age', Arr(10, 20));

  var res := df.Rename('Age', 'Years');

  Check(not res.HasColumn('Age'), 'Old column name must disappear');
  Check(res.HasColumn('Years'), 'New column name must appear');
  Check(res.Schema.NameAt(1) = 'Years', 'Schema must contain new column name');
  Check(res.GetColumn(1).Info.Name = 'Years', 'Physical column name must match schema');
  CheckSchemaMatchesColumns(res, Arr(false, false));
end.
