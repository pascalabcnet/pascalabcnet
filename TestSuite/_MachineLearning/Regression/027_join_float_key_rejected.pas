uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var left := new DataFrame;
  left.AddFloatColumn('id', Arr(1.0, 2.0));
  left.AddStrColumn('name', Arr('A', 'B'));

  var right := new DataFrame;
  right.AddFloatColumn('id', Arr(1.0, 2.0));
  right.AddFloatColumn('score', Arr(10.0, 20.0));

  CheckRaises(procedure -> begin var res := left.Join(right, 'id'); end,
    'Join on float key must be rejected');
end.
