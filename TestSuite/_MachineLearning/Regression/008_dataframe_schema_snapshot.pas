uses MLABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('Msk', 'Spb'));
  df.AddIntColumn('Age', Arr(20, 30));
  df := df.SetCategorical(['City']);

  var s1 := df.Schema;
  var names := s1.ColumnNames;
  var types := s1.Types;
  var cats := s1.CategoricalFlags;

  names[0] := 'Broken';
  types[0] := ColumnType.ctBool;
  cats[0] := false;

  var s2 := df.Schema;

  Check(s2.NameAt(0) = 'City', 'Schema name snapshot must not mutate DataFrame');
  Check(s2.ColumnTypeAt(0) = ColumnType.ctStr, 'Schema type snapshot must not mutate DataFrame');
  Check(s2.IsCategoricalAt(0), 'Schema categorical snapshot must not mutate DataFrame');
end.
