unit u_classconst2;
type
  t1 = class
    public const arr: array of string = ('abc', 'def');
    
    public static function f1: array of string := arr;
    
  end;
    
begin
  
end.