unit u_abstract3;
type
  
  t1 = abstract class
    // обязательно видимость private
     private function f1: byte; abstract;
  end;
  t2 = class(t1)
     private function f1: byte; override := 0;
  end;
  
  
  // обязательно ещё 1 промежуточный тип между t2 и t4
  t3 = class(t2) end;
  
end.