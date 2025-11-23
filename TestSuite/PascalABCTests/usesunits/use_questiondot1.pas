uses u_questiondot1;

begin
  var a: t1 := nil;
  var s := a?.ToString;
  assert(string.IsNullOrEmpty(s));
end.