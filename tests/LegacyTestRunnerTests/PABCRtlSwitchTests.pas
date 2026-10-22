{$reference Compiler.dll}
{$reference CompilerTools.dll}
{$reference Errors.dll}
{$reference LanguageIntegrator.dll}
{$reference TreeConverter.dll}

uses PascalABCCompiler, System.IO, System.Collections, System.Reflection;

begin
  Languages.Integration.LanguageIntegrator.LoadAllLanguages();
  var compiler := new Compiler;
  var suite := Path.GetFullPath(ParamStr(1));
  var helper := typeof(PascalABCCompiler.NetHelper.NetHelper);
  var tables := helper.GetProperty('members', BindingFlags.NonPublic or BindingFlags.Static).GetValue(nil, nil) as IDictionary;
  var receiver := typeof(List<integer>).GetGenericTypeDefinition;
  PascalABCCompiler.NetHelper.NetHelper.GetMembers(receiver, 'Add');
  var originalTable := tables[receiver];
  for var cycle := 1 to 4 do
    foreach var name in Arr('beginend', 'arrays4', 'foreach9') do
    begin
      var options := new CompilerOptions(Path.Combine(suite, name + '.pas'),
        CompilerOptions.OutputType.ConsoleApplicaton);
      options.OutputDirectory := Path.Combine(suite, 'exe');
      Directory.CreateDirectory(options.OutputDirectory);
      options.UseDllForSystemUnits := true;
      options.IgnoreRtlErrors := false;
      options.RunWithEnvironment := false;
      options.Debug := true;
      compiler.ErrorsList.Clear();
      compiler.Warnings.Clear();
      compiler.Compile(options);
      if compiler.ErrorsList.Count <> 0 then
        raise new Exception(name + ': ' + compiler.ErrorsList[0].ToString);
      // A source module deliberately switches compilation to standalone PCUs.
      assert(options.UseDllForSystemUnits = (name <> 'arrays4'));
      assert(object.ReferenceEquals(originalTable, tables[receiver]));
      var operators := PascalABCCompiler.NetHelper.NetHelper.GetMembers(receiver, '+=');
      assert((operators.Count > 0) = options.UseDllForSystemUnits);
    end;
  Println('PASS RTL -> PCU -> RTL four times; metadata tables reused; inactive RTL operators hidden');
end.
