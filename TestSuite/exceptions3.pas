begin
  var i := 0;
  try
    while true do
    begin
      Inc(i); 
      try
        if i > 3 then
          raise new Exception();
        continue;  
      except on e1: Exception do
        break;
      end;
    end;
  except on e: Exception do
  end;
  assert(i = 4);
end.