begin
  var i := 0;
  var p: procedure := ()->try
    raise new Exception('test');
  except 
    on ex: Exception do
    begin
      i := 1;
    end;
      
  end;
  p;
  assert(i = 1);
end.