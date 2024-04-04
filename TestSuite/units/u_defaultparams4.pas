unit u_defaultparams4;

type TClass = class
  static p: integer = 2;
  static function getp: integer := p;
  static property x: integer read p;
  static property y: integer read getp;
end;

function f(i: integer := TClass.x): integer;
begin
  Result := i;
end;

function f2(i: integer := TClass.y): integer;
begin
  Result := i;
end;
begin
  assert(f = 2);
  assert(f2 = 2);
end.