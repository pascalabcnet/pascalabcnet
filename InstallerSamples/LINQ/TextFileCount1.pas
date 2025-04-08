begin
  var d := Dict('begin' to 0);
  var words := ReadAllText('TextFileCount1.pas').ToWords(AllDelimiters); 
  foreach var word in words do
    d[word] := d.Get(word) + 1;
  d.Print(NewLine);    
end.