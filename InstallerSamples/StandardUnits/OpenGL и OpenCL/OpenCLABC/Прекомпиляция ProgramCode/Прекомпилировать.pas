uses OpenCLABC;

begin
  var prog := new ProgramCode(Context.Default, ReadAllText('0.cl'));
  var f := System.IO.File.Create('0.cl.temp_bin');
  prog.SerializeTo(f);
  f.Close;
end.