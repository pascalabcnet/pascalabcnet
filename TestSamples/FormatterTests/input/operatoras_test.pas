{type
  Int=class
    i:integer;
    constructor create;
    begin
      i:=1;
    end;
    function operator*=(j:integer):Int;
    begin
      i:=i*j;
    end;
  end;

var i:int;
  }
begin
  
end.