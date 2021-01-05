type
  t1 = class
    procedure p1<T,U>(x: T; y: U); virtual := exit;
  end;
  
  t2 = class(t1)
    procedure p1<TT,UU>(x: UU; y: TT); override;
    begin
    end;
  end;
  
begin
end.