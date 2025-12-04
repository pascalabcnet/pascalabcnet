begin
  Print(if true then 2 else 3 + if true then 2 else 3);
  var b := 2 * if true then 2 else 3;
  var c := 2 < if true then 2 else 3;
end.