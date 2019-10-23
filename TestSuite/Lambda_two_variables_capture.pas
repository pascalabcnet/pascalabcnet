type
  t1 = class
  public 
    f: integer;
    procedure p1;
    begin
      var v := 2;
      var p: procedure := ()->
      begin
        var nv := v;
        var nf := f;
      end;
    end;
  end;

begin 
end.