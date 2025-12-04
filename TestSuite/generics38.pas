type
  Comp1 = class(System.Collections.Generic.EqualityComparer<byte>)
    
    public function Equals(x, y: byte): boolean; override :=
    x = y;
    
    public function GetHashCode(obj: byte): integer; override :=
    obj.GetHashCode;
    
  end;
  
  t0 = class end;
  
 
  Comp2 = class(System.Collections.Generic.EqualityComparer<t0>)
    
    public function Equals(x, y: t0): boolean; override :=
    x = y;
    
    public function GetHashCode(obj: t0): integer; override :=
    obj.GetHashCode;
    
  end;
  
  Comp<T> = class(System.Collections.Generic.EqualityComparer<T>)
    
    public function Equals(x, y: T): boolean; override :=
    x = y;
    
    public function GetHashCode(obj: T): integer; override :=
    obj.GetHashCode;
    
  end;
  
begin 
  var o := new Comp2;
  var t0obj := new t0;
  assert(o.Equals(new t0, new t0) = false);
  assert(o.Equals(t0obj, t0obj));
  var o2 := new Comp<integer>;
  assert(o2.Equals(2, 2));
end.