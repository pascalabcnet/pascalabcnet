uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('A', 'B', 'C', 'D'));
  df.AddIntColumn('Age', Arr(10, 20, 30, 40));

  var h := df.Head(2);

  Check(h.RowCount = 2, 'Head(2) row count mismatch');
  Check(h.GetStrColumn('City')[0] = 'A', 'Head first row mismatch');
  Check(h.GetStrColumn('City')[1] = 'B', 'Head second row mismatch');
  Check(h.GetIntColumn('Age')[0] = 10, 'Head first age mismatch');
  Check(h.GetIntColumn('Age')[1] = 20, 'Head second age mismatch');
end.
