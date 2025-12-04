begin
  var i: integer;
  var p: procedure(ptr: pointer);
  p := ptr->begin assert(@i = pinteger(ptr)) end;
  p(@i);
end.