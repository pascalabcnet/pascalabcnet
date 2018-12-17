type
  TClass = class
  private
    fX, fY: integer;
    
  public
    property X: integer write (fX, fY) := (3, 4);
  end;

begin
  var d := TClass.Create;
  d.X := 55;
  Assert(d.fX=3);
end.