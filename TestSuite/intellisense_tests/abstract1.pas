type
  t1 = abstract class
    
    procedure p1; abstract;
  
  end;
  
  t2 = class(t1)
  end;
  
  t3{@class t3@} = class(t1)
    procedure p1;
    begin
      
    end;
  end;
  
  t4 = abstract class
    function f1(a:integer): integer; abstract;
  end;
  
  t5{@abstract class t5@} = class(t4)
  end;
  
  t6{@class t6@} = class(t4)
    function f1(a:integer): integer;
    begin
      
    end;
  end;
  
  t7{@abstract class t7@} = class(t4)
    function f1(a:real): integer;
    begin
      
    end;
  end;
  
begin
 var t: t2{@abstract class t2@};
end.