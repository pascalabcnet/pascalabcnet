uses
  System.IO;

begin
  var files := Directory.GetFiles('..\TestSuite\units');
  foreach fname: string in files do
  begin
    if not fname.StartsWith('u_') and not fname.StartsWith('use_') then
    begin
      var progtext := &File.ReadAllText(fname);
      var unit_header := 'unit u_' + Path.GetFileNameWithoutExtension(fname) + ';' + System.Environment.NewLine;
      progtext := unit_header + progtext;
      &File.WriteAllText(fname, progtext);
      &File.Move(fname,Path.Combine(Path.GetDirectoryName(fname),'u_'+Path.GetFileName(fname)));
      &File.WriteAllText('..\TestSuite\usesunits\use_'+Path.GetFileName(fname),string.Format('uses u_{0}; begin end.',Path.GetFileNameWithoutExtension(fname)));
    end;
  end;
end.