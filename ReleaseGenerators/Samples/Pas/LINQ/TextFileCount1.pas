begin
  var d := Dict('begin' => 0);
  var delims := Seq(' ',')','(',';','''',',','.','[',']',#10,#13);
  var words := ReadAllText('TextFileCount1.pas').ToWords(delims); 
  foreach var word in words do
    d[word] := d.Get(word) + 1;
  d.Print(NewLine);    
end.