type c1 = class end;

var i := 0;
begin
  
  var d := new Dictionary<c1, Action0>;
  d.Add(new c1, ()->begin i := 1 end);
  
  foreach var v in d.Values do
    //Ошибка: Невозможно явно преобразовать тип c1 к типу System.Action
    Action0(v).Invoke();
  assert(i = 1);
end.