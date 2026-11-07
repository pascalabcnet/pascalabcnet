unit GenericArrayResizeUnit;

interface

type
  ResizeEntry = record
    Text: string;
    Count: integer;
  end;

  ModuleResizer<T> = class
  public
    procedure Grow(var items: array of T; n: integer);
    begin
      System.Array.Resize(items, n);
    end;
  end;

implementation

end.
