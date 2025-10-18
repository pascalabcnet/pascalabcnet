// None - ни один из элементов не удовлетворяет условию

begin
  var a := Arr(1,2,3,4);
  a.None(x -> x > 5).Print
end.