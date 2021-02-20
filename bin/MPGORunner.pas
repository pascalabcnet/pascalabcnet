// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
uses System, System.IO;
begin
  var psi := new System.Diagnostics.ProcessStartInfo();
  var curdir := GetCurrentDir();
  var pabcpath := curdir.Substring(0, curdir.LastIndexOf(Path.DirectorySeparatorChar));
  var files := Directory.GetFiles(Path.Combine(pabcpath, 'OptimizedAssemblies'));
  foreach var s in files do
  begin
    if Path.GetFileName(s) <> '.gitignore' then
      &File.Delete(s);
  end;
  var mpgo := Path.Combine(pabcpath,'Utils\Performance Tools\mpgo.exe');
  psi.FileName := mpgo;
  var args := new string[2]('-scenario',
                            '"'+Path.Combine(curdir,'pabcnetc.exe')+' '+
                            Path.Combine(pabcpath,'ReleaseGenerators\ForPerformanceOptimization.pas')+'" '+
                            ' -AssemblyList '+Path.Combine(curdir,'Compiler.dll')+' '
                            +Path.Combine(curdir,'TreeConverter.dll')+' '
                            +Path.Combine(curdir,'NETGenerator.dll')+' '
                            +Path.Combine(curdir,'ParserTools.dll')+' '
                            +Path.Combine(curdir,'PascalABCParser.dll')+' '
                            '-outdir '+Path.Combine(pabcpath, 'OptimizedAssemblies'));
  psi.Arguments := string.Join(' ',args);
  //psi.UserName := 'administrator';
  //psi.CreateNoWindow := true;
  psi.UseShellExecute := false;
  var p := new System.Diagnostics.Process();
  p.StartInfo := psi;
  p.Start();
  p.WaitForExit();
  files := Directory.GetFiles(Path.Combine(pabcpath, 'OptimizedAssemblies'));
  foreach var s in files do
  begin
    &File.Copy(s, Path.Combine(curdir,Path.GetFileName(s)), true);
  end;
end.