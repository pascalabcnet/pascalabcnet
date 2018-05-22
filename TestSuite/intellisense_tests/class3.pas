type
  t1 = class
    procedure p1;
    begin
      var a{@var a: integer;@} := f1;
    end;
    
    function f1 := 0;
  end;

begin end.