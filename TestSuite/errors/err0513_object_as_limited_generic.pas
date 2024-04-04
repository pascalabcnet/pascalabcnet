//!Невозможно инстанцировать, так как тип object не является размерным
type
  TLimited<T> = class
    where T: record;
  end;
  
begin
  var l: TLimited<object>;
end.