type
  t1 = class
    public procedure p1; virtual := exit;
  end;
  
  t2 = class(t1)
    public procedure p1; override := exit;
  end;

begin end.