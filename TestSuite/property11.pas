type
  t1<T> = class(System.IEquatable<t1<T>>)
    
    public property Item[ind: integer]: integer read 1; default;
    
    public static procedure p1;
    begin
      var a := new t1<T>;
      //Ошибка: Нет индексного свойства по умолчанию для типа t1<T>
      var x := a[0];
      assert(x = 1);
    end;
    
    public function Equals(other: t1<T>) := true;
    
  end;
  
begin
  t1&<integer>.p1;
end.