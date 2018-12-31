type
  TExample = class
  private 
    fX: integer;
  
  public 
    property X: integer
    read fX
    write
    begin
    if value < 0 then
    raise new System.ArgumentException();
    fX := value;
    end;
 property Y: integer read fX write    begin writeln(2);end;
  end;

begin
end.
