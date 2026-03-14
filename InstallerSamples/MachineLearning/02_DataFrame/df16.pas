// Left Join
uses DataFrameABC;

begin
  var students := DataFrame.FromCsvText('''
  id,name
  1,Alice
  2,Bob
  3,Charlie
  ''');
  
  var scores := DataFrame.FromCsvText('''
  id,score
  1,85
  2,90
  4,70
  ''');
  
  students.Join(scores, 'id', jkLeft).Print;
end.