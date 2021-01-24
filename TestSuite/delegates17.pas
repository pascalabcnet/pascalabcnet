begin
  var a: array of integer := |1,2,3|;
  var l: List<integer> := new List<integer>(|4,2,5|);
  var a2 := a.ConvertAll(l.Contains);//Ошибка: Нельзя преобразовать тип List<byte> к byte
  assert(a2.ArrEqual(|false, true, false|));
end.