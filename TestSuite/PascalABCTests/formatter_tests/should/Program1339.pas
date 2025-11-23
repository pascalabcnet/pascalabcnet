begin
  var x1: Action0 := () -> exit;
  var x2: Func0<byte> := () -> 0;
  
  var y1: Action0 := () -> begin exit end;
  var y2: Func0<byte> := () -> begin Result := 0; end;
end.