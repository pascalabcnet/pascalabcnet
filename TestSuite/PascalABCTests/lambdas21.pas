var i: integer;

procedure p2(f: function(params a: array of integer): integer);
begin
  assert(f(1,2,3) = 6);
  Inc(i);
end;


function f(params a: array of integer): integer;
begin
  Result := a.Sum();
end;
begin

  //Ошибка: Нельзя преобразовать тип function(_: λ_anytype): λ_anytype к function(a: array of byte): byte
  p2(a->f(a));
  assert(i = 1);
end.
