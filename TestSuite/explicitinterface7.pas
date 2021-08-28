//winonly
var i: integer;

type
  I1<T>=interface
    procedure p1;
  end;
  
  C1<T>=class(I1<T>)//Класс C1<T> не реализует метод p1 интерфейса I1<T>
    
    public procedure I1<T>.p1;
    begin
      i := 1;
    end;
    public procedure p1;
    begin
      i := 2;
    end;
  end;

begin 
  var ii: I1<integer> := new C1<integer>;
  var o := new C1<integer>;
  ii.p1;
  assert(i = 1);
  o.p1;
  assert(i = 2);
end.