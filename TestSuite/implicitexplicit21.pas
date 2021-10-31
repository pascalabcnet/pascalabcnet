var i: integer;
type
  Generic1<T> = class end;
  Generic2<T1,T2> = class end;
  
  ConvTarget = class
    
    static function operator implicit<T1, T2>(o: Generic2<T1, Generic1<T2>>): ConvTarget; where T1, T2: record;
    begin
      Result := new ConvTarget;
      i := 1;
    end;
    
  end;
  
begin
  var a: Generic2<byte, Generic1<word>>;
  var o: ConvTarget := a;
  assert(i = 1);
end.