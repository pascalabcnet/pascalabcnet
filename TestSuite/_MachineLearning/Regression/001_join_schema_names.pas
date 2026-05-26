uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var left1 := new DataFrame;
  left1.AddIntColumn('id', Arr(1, 2));
  left1.AddIntColumn('feature1', Arr(10, 20));

  var right1 := new DataFrame;
  right1.AddIntColumn('id', Arr(1, 2));
  right1.AddIntColumn('feature2', Arr(100, 200));

  var joined1 := left1.Join(right1, 'id');
  Check(joined1.HasColumn('feature2'), 'Expected feature2 without prefix');
  Check(not joined1.HasColumn('right_feature2'), 'Unexpected right_feature2 without collision');
  CheckSchemaMatchesColumns(joined1);

  var left2 := new DataFrame;
  left2.AddIntColumn('id', Arr(1, 2));
  left2.AddIntColumn('feature', Arr(10, 20));

  var right2 := new DataFrame;
  right2.AddIntColumn('id', Arr(1, 2));
  right2.AddIntColumn('feature', Arr(100, 200));

  var joined2 := left2.Join(right2, 'id');
  Check(joined2.HasColumn('right_feature'), 'Expected right_feature on collision');
  CheckSchemaMatchesColumns(joined2);
end.
