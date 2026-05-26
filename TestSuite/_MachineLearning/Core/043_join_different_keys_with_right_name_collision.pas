uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var left := new DataFrame;
  left.AddIntColumn('left_id', Arr(1, 2));
  left.AddStrColumn('Name', Arr('A', 'B'));

  var right := new DataFrame;
  right.AddIntColumn('right_id', Arr(1, 2));
  right.AddStrColumn('left_id', Arr('X', 'Y'));
  right := right.SetCategorical(['left_id']);

  var joined := left.Join(right, Arr($'left_id'), Arr($'right_id'), jkInner);

  Check(joined.HasColumn('left_id'), 'Joined DataFrame must keep left key column');
  Check(joined.HasColumn('right_left_id'), 'Joined DataFrame must rename conflicting right non-key column');
  Check(joined.IsCategorical('right_left_id'), 'Renamed right categorical non-key column must stay categorical');
  CheckSchemaMatchesColumns(joined, Arr(false, false, true));
end.
