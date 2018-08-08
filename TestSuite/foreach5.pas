var i: integer;

type
  e1 = class(System.Collections.IEnumerator)
    public function MoveNext := false;
    public function get_current:object := nil;
    public procedure Reset := exit;
  end;
  e2<T> = class(System.Collections.Generic.IEnumerator<T>)
    public function MoveNext := false;
    public function System.Collections.IEnumerator.get_current:object := nil;
    public function get_current:T := default(T);
    public procedure Reset := exit;
    public procedure Dispose := exit;
  end;
  
  t2=class
    b:byte;
  end;
  t1<T> = class(System.Collections.Generic.IEnumerable<T>)
    
    public function System.Collections.IEnumerable.GetEnumerator: System.Collections.IEnumerator;
    begin
      i := 1;
      Result := new e1;
    end;
    
    public function GetEnumerator: System.Collections.Generic.IEnumerator<T>;
    begin
      i := 2;
      Result := new e2<T>;
    end;
  
  end;

begin
  foreach var o in new t1<t2> do
    o.b := 2;
  assert(i = 2);
end.