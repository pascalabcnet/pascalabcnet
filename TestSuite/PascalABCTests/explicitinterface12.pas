type
  t1<T> = class(System.Collections.Generic.IEnumerator<T>)
    
    public property IEnumerator<T>.Current: T read T(object(2));
    public property System.Collections.IEnumerator.Current: object read 3;
    
    public function System.Collections.IEnumerator.MoveNext: boolean;
    begin
      
    end;
    public procedure System.Collections.IEnumerator.Reset;
    begin
      
    end;
    
    public procedure System.IDisposable.Dispose;
    begin
      
    end;
  end;
  
begin 
  var obj := new t1<integer>;
  assert((obj as IEnumerator<integer>).Current = 2);
  assert(integer((obj as System.Collections.IEnumerator).Current) = 3);
end.