type
  I1<T> = interface 
  end;
  
  T1<T> = class(I1<T1<T>>)//нужно использовать T1<T> как тип для шаблона, I1<I1<T1<T>>> тоже сработает
    i: T;
    constructor(i: T);
    begin
      self.i := i;
    end;
    procedure p1;
    begin
      var o:object := self;//Нельзя преобразовать тип T1<T> к object
    end;
  end;

begin 
  var o: T1<integer> := new T1<integer>(2);
  o.p1;
  assert(o.i = 2);
end.