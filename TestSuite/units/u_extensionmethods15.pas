unit u_extensionmethods15;
function operator*<T1, T2, T3> (Self: (T1,T2); v: T3): (T1,T2,T3); extensionmethod;
begin
  Result := (Self[0],Self[1],v);
end;

begin
  var t0 := (1,2);
  var t1 := t0 * System.DateTime.Now.DayOfWeek;
  assert(t1.Item3 = System.DateTime.Now.DayOfWeek);
  var s := 4+Seq(1,2,3);
  assert(s.ToArray()[0]=4);
end.