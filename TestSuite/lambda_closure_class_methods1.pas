type
  C = class
    gr: real;
    procedure class_ttt;
    begin
    end;
  end;
  
  B = class(C)
    procedure SetGen1(value: real);
    begin
      var p: procedure;
      p := procedure->begin 
        class_ttt; 
        {gr := value}
      end
    end;
  end;

begin
  Assert(1=1)
end.