type
  c1<T1,T2> = class
    // Если объявить сначала "where T1" - тоже должно работать
    where T2: IComparable<T1>;
    where T1: T2;
    
    function f1(o: T1): integer;
    begin
      //Ошибка: Неизвестное имя 'CompareTo'
      Result := o.CompareTo(o);
    end;
    
  end;
  
  c2<T1,T2> = class
    // Если объявить сначала "where T1" - тоже должно работать
	  where T1: T2;
    where T2: IComparable<T1>;
   
    
    function f1(o: T1): integer;
    begin
      //Ошибка: Неизвестное имя 'CompareTo'
      Result := o.CompareTo(o);
    end;
    
  end;
  
  c3<T1,T2> = class
    // Если объявить сначала "where T1" - тоже должно работать
	  where T1: T2, System.ICloneable;
    where T2: IComparable<T1>;
   
    
    function f1(o: T1): integer;
    begin
      //Ошибка: Неизвестное имя 'CompareTo'
      var c := o.Clone();
      Result := o.CompareTo(o);
    end;
    
  end;
  
begin
  var a := new c1<string,string>;
  assert(a.f1('abc') = 0);
  
  var b := new c2<string,string>;
  assert(b.f1('abc') = 0);
  
  var c := new c3<string,string>;
  assert(c.f1('abc') = 0);
end.