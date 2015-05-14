begin
  var d := new Dictionary<string,integer>;
  foreach var s in ReadLines('TextFileCount.pas') do
    foreach var word in s.ToWords(' ',':',')','(',';','''',',','.','=','<','>','[',']','+','-') do
      d[word] := d.Get(word) + 1;
  d.Print(NewLine);    
end.