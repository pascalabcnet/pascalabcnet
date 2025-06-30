## uses System.Diagnostics;

function Fix8Code(s: string) := Regex.Replace(s, '(\\\d{3})+', m->
begin
  var bytes := m.Value.ToWords('\').ConvertAll(x->Convert.ToByte(x, 8));
  Result := System.Text.Encoding.UTF8.GetString(bytes);
end);

var no_wait := 'NoWait' in CommandLineArgs;

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
  var write_files_block := procedure(header, tail: string; enumerate: procedure(on_file: Action<string>)) ->
  begin
    var any_found := false;
    enumerate(fname->
    begin
      if not any_found then
      begin
        any_found := true;
        err := true;
        header.Println;
      end;
      $'- {fname}'.Println;
    end);
    if any_found then
    begin
      tail.Println;
      Println('='*30);
    end;
  end;
  
  write_files_block(
    'Ignored index files:',
    '''
    These files were added to git, even though they are marked as ignored
    # If they were intended to be added, likely its folder should be removed from .gitignore (or added to .gitignore exceptions)
    ''',
    on_file -> foreach var fname in index_files.Order do
    begin
      if fname not in ignored_files then continue;
      on_file(fname);
    end
  );
  
  write_files_block(
    'Unversioned files:',
    '''
    These files were found in the local folder, but aren't known to git
    # If you added them manually, you probably want to add them to the git history
    # If they were created automatically during build/tests/setup generation, you probably want them in .gitignore
    ''',
    on_file -> foreach var fname in all_files.Order do
    begin
      if fname in index_files then continue;
      if fname in ignored_files then continue;
      on_file(fname);
    end
  );
  
  if err and no_wait then
  begin
    '''
    HINT: You can run "bin/GitIgnoreTester.pas" manually in your local directory to test .gitignore changes faster
    '''.Println;
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
if not no_wait then
  ReadString('Done');