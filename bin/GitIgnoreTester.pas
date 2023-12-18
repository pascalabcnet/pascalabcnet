## uses System.Diagnostics;

function Fix8Code(s: string) := Regex.Replace(s, '(\\\d{3})+', m->
begin
  var bytes := m.Value.ToWords('\').ConvertAll(x->Convert.ToByte(x, 8));
  Result := System.Text.Encoding.UTF8.GetString(bytes);
end);

// Чтобы удобнее сравнивать вывод этой программы
//Rewrite(output, 'temp.txt');
try
  var dir := GetEXEFileName;
  loop 2 do dir := System.IO.Path.GetDirectoryName(dir);
//  dir.Println;
  
  var index_files := new HashSet<string>;
  begin
    var psi := new ProcessStartInfo('git', 'ls-files');
    psi.WorkingDirectory := dir;
    psi.UseShellExecute := false;
    psi.RedirectStandardOutput := true;
    
    var p := Process.Start(psi);
    p.OutputDataReceived += (o,e)->
    try
      if e.Data=nil then exit;
      var fname := Fix8Code(e.Data);
      if not fname.StartsWith('"') then
        fname := $'"{fname}"';
      if not index_files.Add(fname) then
        raise new System.InvalidOperationException;
    except
      on ex: Exception do Println(ErrOutput, ex);
    end;
    p.BeginOutputReadLine;
    
    p.WaitForExit;
  end;
  //index_files.PrintLines;
  
  var all_files := new HashSet<string>;
  foreach var full_fname in EnumerateAllFiles(dir) do
  begin
    var fname := full_fname.Substring(dir.Length+1).Replace('\','/');
    if fname.StartsWith('.git/') then continue;
    fname := $'"{fname}"';
    if not all_files.Add(fname) then
      raise new System.InvalidOperationException;
  end;
  
  var ignored_files := new HashSet<string>;
  begin
    var psi := new ProcessStartInfo('git', 'check-ignore --no-index --stdin');
    psi.WorkingDirectory := dir;
    psi.UseShellExecute := false;
    psi.RedirectStandardInput := true;
    psi.RedirectStandardOutput := true;
    
    var p := Process.Start(psi);
    
    p.OutputDataReceived += (o,e)->
    try
      if e.Data=nil then exit;
      var fname := Fix8Code(e.Data);
      if not fname.StartsWith('"') then
        fname := $'"{fname}"';
      if not ignored_files.Add(fname) then
        raise new System.InvalidOperationException;
    except
      on ex: Exception do Println(ErrOutput, ex);
    end;
    p.BeginOutputReadLine;
    
    foreach var fname in all_files do
      p.StandardInput.WriteLine(fname);
    p.StandardInput.Close;
    
    p.WaitForExit;
  end;
  
  var err := false;
  foreach var fname in index_files.Order do
  begin
    if fname not in ignored_files then continue;
    $'Ignored index file: {fname}'.Println;
    err := true;
  end;
  foreach var fname in all_files.Order do
  begin
    if fname in index_files then continue;
    if fname in ignored_files then continue;
    $'Unversioned file: {fname}'.Println;
    err := true;
  end;
  System.Environment.ExitCode := Ord(err);
  
except
  on ex: Exception do
  begin
    Println(ErrOutput, ex);
    System.Environment.ExitCode := ex.HResult;
  end;
end;
//output.Close;
if 'NoWait' not in CommandLineArgs then
  ReadString('Done');