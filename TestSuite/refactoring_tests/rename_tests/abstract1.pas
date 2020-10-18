type
  t0 = abstract class
    
    procedure {@}p1; abstract;
    
  end;
  
  t1 = sealed class(t0)
    
    procedure {!}p1; override := exit;
    
  end;
  t2 = sealed class(t0)
    
    procedure {!}p1; override := exit;
    
  end;

begin end.