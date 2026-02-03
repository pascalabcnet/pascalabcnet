// 3.11.1. ToString(digits) для массива вещественных
begin
  var a := ArrRandomReal(5);
  var s := a.ToString(1);
  Print(s);
end.