uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var left := new DataFrame;
  left.AddIntColumn('Id', Arr(1, 2));
  left.AddStrColumn('Name', Arr('A', 'B'));

  var right := new DataFrame;
  right.AddIntColumn('Id', Arr(1, 2));
  right.AddStrColumn('City', Arr('Msk', 'Spb'));
  right := right.SetCategorical(['City']);

  var joined := left.Join(right, 'Id');

  Check(joined.HasColumn('City'), 'Joined DataFrame must contain right non-key column City');
  Check(joined.IsCategorical('City'), 'Right categorical non-key column must stay categorical after Join');
  CheckSchemaMatchesColumns(joined, Arr(false, false, true));
end.
