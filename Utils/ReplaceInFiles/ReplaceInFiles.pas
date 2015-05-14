uses FilesOperations;
var DefsFile, TemplateFile, ResultFile: string;
    defs: System.Collections.Hashtable;
    f: text;
    key,s:string;
    i:integer;
begin
  if CommandLineArgs.Length<>3 then begin
    Writeln('DefsFile TemplateFile ResultFile');
    exit;
  end;
  DefsFile := CommandLineArgs[0];
  TemplateFile := CommandLineArgs[1];
  ResultFile := CommandLineArgs[2];
  defs := new System.Collections.Hashtable;
  assign(f, DefsFile);
  reset(f);
  while not eof(f) do begin
    readln(f,s);
    i := s.IndexOf('=');
    defs.Add(s.Substring(0,i), s.Substring(i+1));
  end;
  Close(f);
  s := ReadFileToEnd(TemplateFile);
  foreach key in defs.Keys do
    s := s.Replace(key, string(defs[key]));
  WriteStringToFile(ResultFile, s);
end.