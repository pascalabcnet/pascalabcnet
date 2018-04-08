type t1 = class end;//любой ссылочный тип

var i: integer;

procedure p1;
begin
  lock new t1 do
  begin
    i := 1;
    exit;
  end;
end;

procedure p2;
begin
  loop 1 do
    lock new t1 do
    begin
      i := 2;
      continue;
    end;
end;

procedure p3;
begin
  loop 1 do
    lock new t1 do
    begin
      i := 3;
      break;
    end;
end;

procedure p4;
label l1;
begin
  lock new t1 do
  begin
    i := 4;
    goto l1;
  end;
  l1:
end;

begin
  p1;
  assert(i = 1);
  p2;
  assert(i = 2);
  p3;
  assert(i = 3);
  p4;
  assert(i = 4);
end.