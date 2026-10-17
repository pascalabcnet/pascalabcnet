{$include ExpectedDiagnosticHeader.inc}

begin
  foreach var commentSymbol in new string[]('//', '#') do
  begin
    var prefix := commentSymbol + '!';
    foreach var ending in new string[](#13#10, #10, #13, '') do
    begin
      assert(ExpectedDiagnosticMessage(prefix + ' expected message  ' + ending, commentSymbol) = 'expected message');
      if ending <> '' then
        assert(ExpectedDiagnosticMessage(prefix + ' expected message  ' + ending + 'program code' + #13#10, commentSymbol) = 'expected message');
      assert(ExpectedDiagnosticMessage(prefix + ending, commentSymbol) = '');
    end;
    assert(ExpectedDiagnosticMessage('', commentSymbol) = '');
    assert(ExpectedDiagnosticMessage('begin end.', commentSymbol) = '');
    assert(ExpectedDiagnosticMessage(commentSymbol + ' normal comment', commentSymbol) = '');
  end;
  Println('PASS diagnostic headers: CRLF, LF, CR, mixed endings and EOF; Pascal and SPython markers');
end.
