type
  SomeType = record(IComparable<SomeType>) 
  public
    function CompareTo(other: SomeType): integer;
    begin
      Result := 2;
    end;
  end;
  
begin
  var obj := new SomeType();
  assert(obj.CompareTo(obj) = 2);
end.