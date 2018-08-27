type
  TMyAttribute<T> = class(System.Attribute)
  end;
  
  ByteAttribute = TMyAttribute<byte>;
  
  [ByteAttribute()]
  TClass = class
  end;
  
begin
end.