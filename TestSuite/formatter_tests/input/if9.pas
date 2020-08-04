begin
  tbx.MouseWheel += (o, e) -> 
  begin
  if e.Delta > 0 then
  Value := Value + 1
  else if e.Delta < 0 then
  Value := Value - 1
  end;
end.