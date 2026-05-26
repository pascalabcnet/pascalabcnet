uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('A', 'B', 'C', 'D'));
  df.AddIntColumn('Age', Arr(10, 20, 30, 40));

  var t := df.Tail(2);

  Check(t.RowCount = 2, 'Tail(2) row count mismatch');
  Check(t.GetStrColumn('City')[0] = 'C', 'Tail first row mismatch');
  Check(t.GetStrColumn('City')[1] = 'D', 'Tail second row mismatch');
  Check(t.GetIntColumn('Age')[0] = 30, 'Tail first age mismatch');
  Check(t.GetIntColumn('Age')[1] = 40, 'Tail second age mismatch');
end.
