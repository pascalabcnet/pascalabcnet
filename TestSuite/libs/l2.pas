library l2;

type
  
  t1 = class
    
    public function m1 := 0;
    
  end;
  
  t2 = class(t1)
    public static i: integer;
    public static procedure m1;
    begin
      i := 1;
    end;
    
  end;
  
end.