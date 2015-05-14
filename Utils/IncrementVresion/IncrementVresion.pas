uses FilesOperations;
var DefsFile, IncrementDef, name: string;
    IncrementValue: integer;
    defs: System.Collections.Hashtable;
    f: text;
    key,s:string;
    i:integer;
begin
  if CommandLineArgs.Length<>3 then begin
    Writeln('DefsFile IncrementDef IncrementValue');
    exit;
  end;
  DefsFile := CommandLineArgs[0];
  IncrementDef := '%'+CommandLineArgs[1]+'%';
  IncrementValue := System.Convert.ToInt32(CommandLineArgs[2]);
  defs := new System.Collections.Hashtable;
  AssignFile(f, DefsFile);
  reset(f);
  while not eof(f) do begin
    readln(f,s);
    i := s.IndexOf('=');
    name := s.Substring(0,i);
    if name = IncrementDef then
      defs.Add(name, System.Convert.ToInt32(s.Substring(i+1))+IncrementValue)
    else
      defs.Add(name, s.Substring(i+1));
  end;
  CloseFile(f);
  AssignFile(f, DefsFile);
  Rewrite(f);
  foreach key in defs.Keys do
    Writeln(f, key, '=', defs[key]);
  CloseFile(f);
end.