type
  t0<T> = class
    
    l: List<(T, byte)>;
    
  end;
  
  t1 = class(t0<word>)
    
    public procedure p1;
    begin
      l{@var t0<>.l: List<Tuple<word, byte>>;@} := l;
    end;
    
  end;

begin end.