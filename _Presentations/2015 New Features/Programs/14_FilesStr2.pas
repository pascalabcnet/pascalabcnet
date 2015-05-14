begin
  var f := OpenRead('freqs.txt');  
  while not f.Eof do
  begin
    var ss := f.ReadlnString.ToWords;
    if ss[3] = 'verb' then
      writeln(ss[2]);
  end;
  f.Close;
end.