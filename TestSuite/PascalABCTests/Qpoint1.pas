// #2280
begin
  var a: sequence of object := Seq(object(1));
  //Ошибка: Левый операнд операции ?. должен иметь ссылочный тип
  var o := a?.First;
  Assert(o.Equals(1));
end.