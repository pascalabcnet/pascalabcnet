type
  t1 = class
    procedure p1; virtual := exit;
  end;
  
  t2 = class(t1)
    procedure p1; override := exit;
  end;

begin end.