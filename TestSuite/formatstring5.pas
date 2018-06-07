begin
  var t := System.DateTime.Now;
  var s1 := string.Format('date is {0:dd.MM.yy}',t);
  var s2 := $'date is {t:dd.MM.yy}';
  assert(s1 = s2);
end.