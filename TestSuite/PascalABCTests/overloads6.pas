var s: string;

procedure p(x: longword);
begin
  s := 'longword';
end;

procedure p(x: integer);
begin
  s := 'integer';
end;

procedure p1(x: integer);
begin
  s := 'integer';
end;

procedure p1(x: longword);
begin
  s := 'longword';
end;

procedure p2(x: smallint);
begin
  s := 'smallint';
end;

procedure p2(x: longword);
begin
  s := 'longword';
end;


begin
  var a: shortint := -128;
  var b: smallint := -128;
  p(a);
  assert(s='integer');
  p(b);
  assert(s='integer');
  p1(a);
  assert(s='integer');
  p2(a);
  assert(s='smallint');
  p2(b);
  assert(s='smallint');
end.