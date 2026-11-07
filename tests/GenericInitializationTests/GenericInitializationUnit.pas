unit GenericInitializationUnit;

interface

type
  UnitBox<T> = class
  public
    Value: T;
    function ReadLocal: T;
    begin
      var item: T;
      Result := item;
    end;
  end;

implementation

end.
