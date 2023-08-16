type
  d1<T> = Action<procedure(a: T)>;
  
procedure p0(d: d1<byte>) := exit;

var i: integer;
begin
  var d: d1<string>;
  //Ошибка: Нельзя преобразовать тип string к byte
  d := p->p('abc');
  d((x: string)->begin assert(x = 'abc'); i := 1 end);
  assert(i = 1);
end.