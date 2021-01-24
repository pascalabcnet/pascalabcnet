uses System;
var i: integer;

type
  
  C1<T>=class(IComparable<T>)//Класс C1<T> не реализует метод p1 интерфейса I1<T>
    
    public function IComparable<T>.CompareTo(item: T): integer;
    begin
      Result := 1;
    end;
    public function CompareTo(item: T): integer;
    begin
      Result := -1;
    end;
  end;

begin 
  var ii: IComparable<integer> := new C1<integer>;
  var o := new C1<integer>;
  assert(ii.CompareTo(1) = 1);
  assert(o.CompareTo(1) = -1);
end.