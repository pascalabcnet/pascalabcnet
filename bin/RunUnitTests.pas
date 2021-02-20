{$reference nunit.framework.dll}

uses NUnit.Framework.Api;
uses NUnit.Framework.Internal;
uses NUnit.Framework.Interfaces;
uses System.Globalization;
uses System;

var _reportIndex := 0;

function GetResultStatus(resState: ResultState): string;
begin
	var text := resState.Label;
	if string.IsNullOrEmpty(text) then
		text := resState.Status.ToString;
	if (text = 'Failed') or (text = 'Error') then
	begin
		var text2 := resState.Site.ToString();
		if (text2 = 'SetUp') or (text2 = 'TearDown') then
			text := text2 + ' ' + text;
	end;
	Result := text;
end;

var Trim_Chars := Arr(#13,#10);

var RusTestStatus := Dict&<TestStatus,string>(
  (TestStatus.Failed,'Тесты не прошли'),
  (TestStatus.Inconclusive,'Тесты не завершены'),
  (TestStatus.Passed,'Тесты прошли'),
  (TestStatus.Skipped,'Тесты пропущены'),
  (TestStatus.Warning,'Предупреждение')
);

var RusAssertionStatus := Dict&<AssertionStatus,string>(
  (AssertionStatus.Failed,'Тест не прошёл'),
  (AssertionStatus.Inconclusive,'Тесты не завершены'),
  (AssertionStatus.Passed,'Тесты прошли'),
  (AssertionStatus.Error,'Ошибка'),
  (AssertionStatus.Warning,'Предупреждение')
);

procedure DisplayTestResult(prefix, status, fullName, message, stackTrace: string);
begin
  Writeln($'{prefix}) {status}: {fullName}');
	if not string.IsNullOrEmpty(message) then
		Writeln(message.TrimEnd(TRIM_CHARS));
	//if not string.IsNullOrEmpty(stackTrace) then
	//	Writeln(stackTrace.TrimEnd(TRIM_CHARS));
end;

procedure DisplayTestResult(result: ITestResult);
begin
  var resultState := result.ResultState;
	var fullName := result.FullName;
	var message := result.Message;
	var stackTrace := result.StackTrace;
	_reportIndex += 1;
	var text := _reportIndex.ToString;
	var count := result.AssertionResults.Count;
	if count > 0 then
	begin
		var num := 0;
		var prefix := text;
		foreach var assertionResult in result.AssertionResults do
		begin
			if count > 1 then
				prefix := $'{text}-{++num}';
			DisplayTestResult(prefix, RusAssertionStatus[assertionResult.Status], fullName, assertionResult.Message, assertionResult.StackTrace);
		end
	end
	else
	begin
		var resultStatus := GetResultStatus(resultState);
		//if message.StartsWith('No test fixtures were found') then
    //  message := 'Тесты не найдены';

		DisplayTestResult(text, resultStatus, fullName, message, stackTrace);
	end
end;

procedure DisplayTestResults(result: ITestResult);
begin
  var flag := (result.ResultState.Status = TestStatus.Failed) or (result.ResultState.Status = TestStatus.Warning);
	if (result.Test.IsSuite) then
	begin
		if flag then
		begin
			var obj := result.Test as TestSuite;
			var site := result.ResultState.Site;
			if (obj.TestType = 'Theory') or (site = FailureSite.SetUp) or (site = FailureSite.TearDown) then
				DisplayTestResult(result);
			if site = FailureSite.SetUp then
				exit;
		end;
		foreach var child in result.Children do
			DisplayTestResults(child);
	end
	else if (flag) then
		DisplayTestResult(result);
end;


type ResultSummary = class
	TestCount,PassCount,FailureCount,WarningCount,ErrorCount,InconclusiveCount,InvalidCount,SkipCount,IgnoreCount,ExplicitCount,InvalidTestFixtures: integer;
	ResState: ResultState;
	StartTime,EndTime: DateTime;
	Duration: real;

	function RunCount := PassCount + ErrorCount + FailureCount + InconclusiveCount;
	function NotRunCount := InvalidCount + SkipCount + IgnoreCount + ExplicitCount;
	function FailedCount := FailureCount + InvalidCount + ErrorCount;
	function TotalSkipCount := SkipCount + IgnoreCount + ExplicitCount;

	constructor (result: ITestResult);
	begin
		InitializeCounters();
		ResState := result.ResultState;
		StartTime := result.StartTime;
		EndTime := result.EndTime;
		Duration := result.Duration;
		Summarize(result);
	end;

	procedure InitializeCounters();
	begin
		TestCount := 0;
		PassCount := 0;
		FailureCount := 0;
		WarningCount := 0;
		ErrorCount := 0;
		InconclusiveCount := 0;
		SkipCount := 0;
		IgnoreCount := 0;
		ExplicitCount := 0;
		InvalidCount := 0;
	end;

	procedure Summarize(result: ITestResult );
	begin
		var &label := result.ResultState.Label;
		var status := result.ResultState.Status;
		if result.Test.IsSuite then
		begin
			if (status = TestStatus.Failed) and (&label = 'Invalid') then
				InvalidTestFixtures+=1;
			foreach var child in result.Children do
				Summarize(child);
		end
		else
		begin
			TestCount+=1;
			case status of
			TestStatus.Passed: PassCount+=1;
			TestStatus.Skipped:
				if &label = 'Ignored' then
					IgnoreCount+=1
				else if &label = 'Explicit' then
					ExplicitCount+=1
				else
					SkipCount+=1;
			TestStatus.Warning:
				WarningCount+=1;
			TestStatus.Failed:
				if &label = 'Invalid' then
					InvalidCount+=1
				else if &label = 'Error' then
					ErrorCount+=1
				else
					FailureCount+=1;
			TestStatus.Inconclusive: InconclusiveCount+=1;
			end;
		end;
	end;	
end;

procedure DisplaySummaryReport(summary: ResultSummary);
begin
	var status := summary.ResState.Status;
	var text := RusTestStatus[status];
	if text = RusTestStatus[TestStatus.Skipped] then
		text := RusTestStatus[TestStatus.Warning];
	{var num := 0;
	case status of
  	TestStatus.Skipped: num := 10;
  	TestStatus.Failed: num := 9;
  	TestStatus.Passed: num := 8;
  	else num := 4;
	end;}
	
  Writeln;
	Println('Итог теста');
	Println('  Итоговый результат:', text);
	Print('  Количество тестов:', summary.TestCount);
	Print(', Прошло:', summary.PassCount);
	Print(', Не прошло:', summary.FailedCount);
	Print(', Предупреждений:', summary.WarningCount);
	Println;
	if summary.FailedCount > 0 then
	begin
		Print('    Неудачные тесты - Не прошло:', summary.FailureCount);		
		Print(', Ошибки:', summary.ErrorCount);
		Print(', Неверные:', summary.InvalidCount);
		Println;
	end;
	if summary.TotalSkipCount > 0 then
	begin
		Print('    Пропущенные тесты - Пропущено: ', summary.IgnoreCount);
		Print(', Явных: ', summary.ExplicitCount);
		Print(', Других: ', summary.SkipCount);
		Println;
	end;
	Println('  Начало теста:', summary.StartTime.ToString('T',CultureInfo.CreateSpecificCulture('ru-RU')));
	Println('  Конец теста :', summary.EndTime.ToString('T',CultureInfo.CreateSpecificCulture('ru-RU')));
	Println('  Продолжительность:',summary.Duration.Round(3),'секунд');
	Writeln;
end;


begin
  var fname := ParamStr(1);//'33.exe';
  var runner := new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());
  var filter := TestFilter.Empty;
  var runSettings := Dict&<string,object>(('WorkDirectory',object('.')));
  
  runner.Load(fname,runSettings);
  var TestResult := runner.Run(nil,filter);

  //Println($'Не прошло тестов: {TestResult.FailCount}');
  //Println($'Прошло тестов: {TestResult.PassCount}');
  //Println;

  var Summary := new ResultSummary(TestResult);
  if Summary.TestCount = 0 then
  begin
    Println($'Файл {ParamStr(1)}: тесты не найдены');
    Println;
    Println('Минимальный файл, содержащий тесты:'#13#10
      ''#13#10
      'uses NUnitABC;'#13#10
      ''#13#10
      '[Test]'#13#10
      'procedure Test1;'#13#10
      'begin'#13#10
      '  Assert.AreEqual(2*2,4);'#13#10
      'end;'#13#10
      ''#13#10
      'begin'#13#10
      'end.' 
    );
    
  end
  else
  begin
    DisplayTestResults(TestResult);
    DisplaySummaryReport(Summary);
  end;
    
  
  readln;
end.