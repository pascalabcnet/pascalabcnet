type
  t1 = class
    procedure p1<TTT>; virtual;
    where TTT: constructor;
    begin
    end;
  end;
  
  t2 = class(t1)
    procedure p1<T>; override;
    where T: record, constructor;
    begin
      //Assert(typeof(T).IsValueType);
    end;
  end;
  
begin
  var a: t1 := new t2;
  a.p1&<object>;
end.