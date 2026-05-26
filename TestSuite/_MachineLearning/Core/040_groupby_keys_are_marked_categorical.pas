uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddBoolColumn('Flag', Arr(true, false, true));
  df.AddFloatColumn('X', Arr(1.0, 2.0, 3.0));
  df := df.SetCategorical(['Flag']);

  var g := df.GroupBy(['Flag']).Count;

  Check(g.IsCategorical('Flag'), 'GroupBy key must stay categorical');
  CheckSchemaMatchesColumns(g, Arr(true, false));
end.
