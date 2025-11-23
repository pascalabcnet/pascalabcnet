begin
  var e: System.InvalidCastException;
  try
    var i := real(object(integer(5)));
    i := 2;
  except on ex: Exception do
    e := ex as System.InvalidCastException;
  end;
  assert(e <> nil);
end.