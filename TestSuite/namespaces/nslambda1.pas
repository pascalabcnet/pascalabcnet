namespace nslambda1;

type
  T1 = class
    public static function Contains(s: string; c: char) := s.Any(x -> x = c);
  end;
  
  t2 = class
    
    const c1 = 5;
    
    procedure p1;
    begin
      var arr1 := ArrGen(1, i -> c1);
      assert(arr1[0] = 5);
    end;
    
  end;

end.