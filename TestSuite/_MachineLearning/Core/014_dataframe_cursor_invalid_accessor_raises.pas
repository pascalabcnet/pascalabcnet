uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddIntColumn('Age', Arr(20, 30));

  var cur := df.GetCursor;
  Check(cur.MoveNext, 'Cursor must move to first row');

  CheckRaises(procedure -> begin var s := cur.Str(0); end,
    'Cursor.Str on int column must raise');
  CheckRaises(procedure -> begin var b := cur.Bool(0); end,
    'Cursor.Bool on int column must raise');
end.
