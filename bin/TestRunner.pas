// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
{$reference Compiler.dll}
{$reference CodeCompletion.dll}
{$reference Errors.dll}
{$reference CompilerTools.dll}
{$reference System.Windows.Forms.dll}

uses PascalABCCompiler, System.IO, System.Diagnostics;

var TestSuiteDir: string;

function GetTestSuiteDir: string;
begin
  var dir := Path.GetDirectoryName(GetEXEFileName());
  var ind := dir.LastIndexOf('bin');
  Result := dir.Substring(0,ind)+'TestSuite';
end;

function GetLibDir: string;
begin
  var dir := Path.GetDirectoryName(GetEXEFileName());
  Result := dir+'\Lib';
end;

procedure CompileErrorTests(withide: boolean);
begin
  
  var comp := new Compiler();
  
  var files := Directory.GetFiles(TestSuiteDir+'\errors','*.pas');
  for var i := 0 to files.Length - 1 do
  begin
    var co: CompilerOptions := new CompilerOptions(files[i],CompilerOptions.OutputType.ConsoleApplicaton);
    co.Debug := true;
    co.OutputDirectory := TestSuiteDir+'\errors';
    co.UseDllForSystemUnits := false;
    co.RunWithEnvironment := withide;
    comp.ErrorsList.Clear();
    comp.Warnings.Clear();
    
    comp.Compile(co);
    if comp.ErrorsList.Count = 0 then
    begin
      //System.Windows.Forms.MessageBox.Show('Compilation of error sample '+files[i]+' was successfull'+System.Environment.NewLine);
      //Halt();
    end
    else if comp.ErrorsList.Count = 1 then
    begin
      if comp.ErrorsList[0].GetType() = typeof(PascalABCCompiler.Errors.CompilerInternalError) then
        System.Windows.Forms.MessageBox.Show('Compilation of '+files[i]+' failed'+System.Environment.NewLine+comp.ErrorsList[0].ToString());
    end;
  end;
  
end;

procedure CompileAllRunTests(withdll: boolean; only32bit: boolean := false);
begin
  
  var comp := new Compiler();
  
  var files := Directory.GetFiles(TestSuiteDir,'*.pas');
  for var i := 0 to files.Length - 1 do
  begin
    var co: CompilerOptions := new CompilerOptions(files[i],CompilerOptions.OutputType.ConsoleApplicaton);
    co.Debug := true;
    co.OutputDirectory := TestSuiteDir+'\exe';
    co.UseDllForSystemUnits := withdll;
    co.RunWithEnvironment := false;
    co.IgnoreRtlErrors := false;
    co.Only32Bit := only32bit;
    comp.ErrorsList.Clear();
    comp.Warnings.Clear();
    
    comp.Compile(co);
    if comp.ErrorsList.Count > 0 then
    begin
      System.Windows.Forms.MessageBox.Show('Compilation of '+files[i]+' failed'+System.Environment.NewLine+comp.ErrorsList[0].ToString());
      Halt();
    end;
  end;
  
end;

procedure CompileAllCompilationTests(dir: string; withdll: boolean);
begin
  
  var comp := new Compiler();
  
  var files := Directory.GetFiles(TestSuiteDir+'\'+dir,'*.pas');
  for var i := 0 to files.Length - 1 do
  begin
    var co: CompilerOptions := new CompilerOptions(files[i],CompilerOptions.OutputType.ConsoleApplicaton);
    co.Debug := true;
    co.OutputDirectory := TestSuiteDir+'\'+dir;
    co.UseDllForSystemUnits := withdll;
    co.RunWithEnvironment := false;
    co.IgnoreRtlErrors := false;
    comp.ErrorsList.Clear();
    comp.Warnings.Clear();
    
    comp.Compile(co);
    if comp.ErrorsList.Count > 0 then
    begin
      System.Windows.Forms.MessageBox.Show('Compilation of '+files[i]+' failed'+System.Environment.NewLine+comp.ErrorsList[0].ToString());
      Halt();
    end;
  end;
  
end;

procedure CompileAllUnits;
begin
  var comp := new Compiler();
  var files := Directory.GetFiles(TestSuiteDir+'\units','*.pas');
  var dir := TestSuiteDir+'\units\';
  for var i := 0 to files.Length - 1 do
  begin
    var co: CompilerOptions := new CompilerOptions(files[i],CompilerOptions.OutputType.ConsoleApplicaton);
    co.Debug := true;
    co.OutputDirectory := dir;
    co.UseDllForSystemUnits := false;
    comp.ErrorsList.Clear();
    comp.Warnings.Clear();
    
    comp.Compile(co);
    if comp.ErrorsList.Count > 0 then
    begin
      System.Windows.Forms.MessageBox.Show('Compilation of '+files[i]+' failed'+System.Environment.NewLine+comp.ErrorsList[0].ToString());
      Halt();
    end;
  end;
  
end;

procedure CompileAllUsesUnits;
begin
  System.Environment.CurrentDirectory := Path.GetDirectoryName(GetEXEFileName());
  var comp := new Compiler();
  
  var files := Directory.GetFiles(TestSuiteDir+'\usesunits','*.pas');
  for var i := 0 to files.Length - 1 do
  begin
    var co: CompilerOptions := new CompilerOptions(files[i],CompilerOptions.OutputType.ConsoleApplicaton);
    co.Debug := true;
    co.OutputDirectory := TestSuiteDir+'\exe';
    co.UseDllForSystemUnits := false;
    comp.ErrorsList.Clear();
    comp.Warnings.Clear();
    comp.Compile(co);
    if comp.ErrorsList.Count > 0 then
    begin
      System.Windows.Forms.MessageBox.Show('Compilation of '+files[i]+' failed'+System.Environment.NewLine+comp.ErrorsList[0].ToString());
      Halt();
    end;
  end;
  
end;

procedure CopyPCUFiles;
begin
  System.Environment.CurrentDirectory := Path.GetDirectoryName(GetEXEFileName());
  var files := Directory.GetFiles(TestSuiteDir+'\units','*.pcu');
  
  foreach fname: string in files do
  begin
    &File.Move(fname,TestSuiteDir+'\usesunits\'+Path.GetFileName(fname));
  end;
end;

procedure RunAllTests(redirectIO: boolean);
begin
  var files := Directory.GetFiles(TestSuiteDir+'\exe','*.exe');
  for var i := 0 to files.Length - 1 do
  begin
      var psi := new System.Diagnostics.ProcessStartInfo(files[i]);
      psi.CreateNoWindow := true;
		  psi.UseShellExecute := false;
		  
		  psi.WorkingDirectory := TestSuiteDir+'\exe';
		  {psi.RedirectStandardInput := true;
		  psi.RedirectStandardOutput := true;
		  psi.RedirectStandardError := true;}
		  var p: Process := new Process();
		  p.StartInfo := psi;
		  p.Start();
		  if redirectIO then
        p.StandardInput.WriteLine('GO');
		  //p.StandardInput.AutoFlush := true;
		  //var p := System.Diagnostics.Process.Start(psi);
		  
		  while not p.HasExited do
		    Sleep(10);
		  if p.ExitCode <> 0 then
		  begin
		    System.Windows.Forms.MessageBox.Show('Running of '+files[i]+' failed. Exit code is not 0');
		    Halt;
		  end;
  end;
end;

procedure RunExpressionsExtractTests;
begin
  CodeCompletion.CodeCompletionTester.Test();  
end;

procedure RunFormatterTests;
begin
  CodeCompletion.FormatterTester.Test();
  var errors := &File.ReadAllText(TestSuiteDir+'\formatter_tests\output\log.txt');
  if not string.IsNullOrEmpty(errors) then
  begin
    System.Windows.Forms.MessageBox.Show(errors+System.Environment.NewLine+'more info at TestSuite\formatter_tests\output\log.txt');
    Halt;
  end;
end;

procedure ClearDirByPattern(dir, pattern: string);
begin
  var files := Directory.GetFiles(dir, pattern);
  for var i := 0 to files.Length - 1 do
  begin
    try
      if Path.GetFileName(files[i]) <> '.gitignore' then
        &File.Delete(files[i]);
    except
    end;
  end;
end;

procedure ClearExeDir;
begin
  ClearDirByPattern(TestSuiteDir+'\exe','*.*');
  ClearDirByPattern(TestSuiteDir+'\CompilationSamples','*.exe');
  ClearDirByPattern(TestSuiteDir+'\CompilationSamples','*.pdb');
  ClearDirByPattern(TestSuiteDir+'\CompilationSamples','*.pcu');
  ClearDirByPattern(TestSuiteDir+'\pabcrtl_tests','*.exe');
  ClearDirByPattern(TestSuiteDir+'\pabcrtl_tests','*.pdb');
  ClearDirByPattern(TestSuiteDir+'\pabcrtl_tests','*.pcu');
end;

procedure DeletePCUFiles;
begin
  ClearDirByPattern(TestSuiteDir+'\usesunits','*.pcu');
end;

procedure DeletePABCSystemPCU;
begin
  var dir := Path.Combine(Path.GetDirectoryName(GetEXEFileName()),'Lib');
  var pcu := Path.Combine(dir,'PABCSystem.pcu');
end;

procedure CopyLibFiles;
begin
  var files := Directory.GetFiles(GetLibDir(),'*.pas');
  foreach f: string in files do
  begin
    &File.Copy(f, TestSuiteDir+'\CompilationSamples\'+Path.GetFileName(f), true);
  end;
end;

begin
  //DeletePABCSystemPCU;
  TestSuiteDir := GetTestSuiteDir;
  System.Environment.CurrentDirectory := Path.GetDirectoryName(GetEXEFileName());
  DeletePCUFiles;
  ClearExeDir;
  CompileAllRunTests(false);
  CopyLibFiles;
  CompileAllCompilationTests('CompilationSamples',false);
  CompileAllUnits;
  CopyPCUFiles;
  CompileAllUsesUnits;
  CompileErrorTests(false);
  writeln('Tests compiled successfully');
  RunAllTests(false);
  writeln('Tests run successfully');
  ClearExeDir;
  DeletePCUFiles;
  CompileAllRunTests(true);
  writeln('Tests with pabcrtl compiled successfully');
  CompileAllCompilationTests('pabcrtl_tests',true);
  RunAllTests(false);
  writeln('Tests with pabcrtl run successfully');
  ClearExeDir;
  CompileAllRunTests(false, true);
  writeln('Tests in 32bit mode compiled successfully');
  RunAllTests(false);
  writeln('Tests in 32bit run successfully');
  System.Environment.CurrentDirectory := Path.GetDirectoryName(GetEXEFileName());
  RunExpressionsExtractTests;
  writeln('Intellisense expression tests run successfully');
  RunFormatterTests;
  writeln('Formatter tests run successfully');
end.