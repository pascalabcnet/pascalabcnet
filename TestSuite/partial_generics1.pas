type
  T1<T> = partial class
    i: integer;
    procedure Test;
    begin
      Inc(i);    
    end;
  end;
  
  T1<T> = partial class
    procedure Test2;
    begin
      Inc(i);
    end;
  end;

begin
  var obj := new T1<integer>;
  obj.Test;
  obj.Test2;
  assert(obj.i = 2);
end.