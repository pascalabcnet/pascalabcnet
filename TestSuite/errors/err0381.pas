type
  t1 = class(List<byte>)
    
    public procedure Add(item: byte); override;
    begin
      inherited Add(item);
    end;
    
  end;
  
begin
  new t1;
end.