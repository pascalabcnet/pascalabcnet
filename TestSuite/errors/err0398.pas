var i: integer;

type
  I1<T>=interface
    procedure p1;
  end;
  C1<T>=class(I1<T>)
    
    public procedure I1<T>.p1(i: integer);
    begin
      i := 1;
    end;
   
  end;

begin 

end.