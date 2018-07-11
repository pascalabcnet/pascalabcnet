type
  T1 = auto class
    X1: integer;
    
    function Clone() := new T1(X1);
  end;
  
begin 
  Assert((new T1(2)).Clone.X1 = 2)
end.