type
  i0 = interface end;
  
  base_i<TSelf> = interface
  where TSelf: base_i<TSelf>;
    function fi(a: TSelf): TSelf;
  end;
  
  base_c<TSelf> = abstract class
  where TSelf: base_c<TSelf>;
    function fc(a: TSelf): TSelf; abstract;
  end;
  
  class1<T> = class(i0, base_i<class1<T>>)
    public function fi(a: class1<T>) := a;
  end;
  class2<T> = class(base_c<class2<T>>, base_i<class2<T>>)
    public function fi(a: class2<T>) := self;
    public function fc(a: class2<T>): class2<T>; override := self;
  end;
  
  record1<T> = record(i0, base_i<record1<T>>)
    public function fi(a: record1<T>) := self;
  end;
  
  interface1<T> = interface(i0, base_i<interface1<T>>)
    
  end;
  
begin end.