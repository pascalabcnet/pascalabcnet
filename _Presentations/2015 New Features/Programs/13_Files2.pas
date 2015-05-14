begin
  var f := OpenRead('13_Files2.pas');
  while not f.Eof do
  begin
    var s := f.ReadlnString;
    writeln(s);
  end;
  f.Close;
end.