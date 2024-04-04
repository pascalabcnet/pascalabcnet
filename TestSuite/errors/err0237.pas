//!Класс t2 абстрактный и не может иметь атрибут sealed, потому что метод test не реализован
type
  t1 = abstract class 
    procedure test; abstract;
  end;
  t2 = sealed class(t1) 
    
  end;
  
begin end.