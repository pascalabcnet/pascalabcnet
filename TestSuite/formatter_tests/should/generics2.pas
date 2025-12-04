type
  list<T> = class
    procedure Add(a: T);
    begin
      assert(a.Equals(5));
    end;
    
    procedure Add2(a: T); virtual;
    begin
      
    end;
  end;
  
  My = class(list<integer>)
    procedure Add2(a: integer); override;
    begin
      assert(a.Equals(3));
    end;
  end;

begin
  var m := new My;
  m.Add(5);
  m.Add2(3);
end.