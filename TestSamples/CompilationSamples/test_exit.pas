{procedure exit;
begin
  writeln('Exit');
end;
}

procedure test;
begin
  exit;
  writeln('ERROR!');
end;

begin
  test;
  writeln('OK');
  sleep(500);
  exit;
  writeln('ERROR!');
  readln;
end.