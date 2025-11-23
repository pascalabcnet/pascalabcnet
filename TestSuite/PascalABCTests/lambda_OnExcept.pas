type MyException = class(Exception) end;

procedure p1;
begin
  
  var i := 8;
  var p: procedure := ()->
  begin
    i := 3;
  end;
  
  try
    raise new MyException;
  except
    on e: Exception do
    begin
      var q: procedure := ()->
      begin
        Print(e);
      end;
      //q;
    end;
    on e: MyException do
    begin
      var q: procedure := ()->
      begin
        Print(e);
      end;
      //q;
    end;
  end;
  
end;

begin 
  p1;
end.