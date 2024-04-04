// issue#2725

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
  
  class1 = class(i0, base_i<class1>)
    public function fi(a: class1) := a;
  end;
  class2 = class(base_c<class2>, base_i<class2>)
    public function fi(a: class2) := self;
    public function fc(a: class2): class2; override := self;
  end;
  
  record1 = record(i0, base_i<record1>)
    public function fi(a: record1) := self;
  end;
  
  interface1 = interface(i0, base_i<interface1>)
    
  end;
  
begin end.