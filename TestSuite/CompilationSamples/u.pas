unit u;

type
  e1 = (
  
    ef1,
    ef2
    
  );
  
  t2 = class;
  tarr = array[e1] of t2;
  t2 = class
  end;
  
  t1 = class
    
    a: array[e1] of t1;//обязательно тип элементов t1
    procedure test(t: tarr);
    begin
      
    end;
  end;
  
end.