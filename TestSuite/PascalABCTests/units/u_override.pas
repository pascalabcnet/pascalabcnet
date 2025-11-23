unit u_override;

type
  
  tBase1 = abstract class
    
    public procedure p1; abstract;
    
    public procedure p2; abstract;
    
  end;
  
  tBase2 = abstract class(tBase1)
    
    public procedure p2; override := exit;
    
  end;
  
end.