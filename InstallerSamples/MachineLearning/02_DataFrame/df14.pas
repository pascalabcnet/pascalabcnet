// GroupBy + Describe
uses DataFrameABC;

begin
  var df := DataFrame.FromCsvText('''
  name,age,score
  Alice,20,85
  Santa,20,90
  Max,21,78
  Bob,22,90
  John,22,96
  Clara,,78
  Sara,22,77
  Kat,21,NA
  ''');
  
  df.GroupBy('age').Describe('score').Print;
end.