type a = class
  procedure p(a:integer); virtual;
  begin
    
  end;
end;

type b = class(a)
  procedure p(var a:integer); override;
  begin
    
  end;
end;

begin
  
end.