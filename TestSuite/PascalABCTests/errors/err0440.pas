//!Нельзя преобразовать тип Generic2<byte,Generic1<string>> к ConvTarget
type
  Generic1<T> = class end;
  Generic2<T1,T2> = class end;
  
  ConvTarget = class
    
    static function operator implicit<T1, T2>(o: Generic2<T1, Generic1<T2>>): ConvTarget; where T1, T2: record;
    begin
      Result := new ConvTarget;
    end;
    
  end;
  
begin
  var a: Generic2<byte, Generic1<string>>;
  var o: ConvTarget := a;
end.