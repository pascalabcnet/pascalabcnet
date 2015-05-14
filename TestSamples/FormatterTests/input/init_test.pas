unit init_test;

procedure proc1;
begin
  writeln('proc1 called');
end;

initialization
  writeln('initialization called');
  proc1;
finalization
  writeln('finalization called');
end.