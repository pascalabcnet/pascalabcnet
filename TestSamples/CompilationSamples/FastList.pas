type 
  List<T> = template class
  beg : System.IntPtr;
  ptr : System.IntPtr;
  
  constructor;
  begin
    ptr := System.Runtime.InteropServices.Marshal.AllocHGlobal(100);
    beg := ptr;
  end;
  procedure Add(el : T);
  type TPtr = ^T;
  begin
    var p := TPtr(ptr.ToPointer);
    p^ := el;
    ptr := System.IntPtr(ptr.ToInt32 + sizeof(T));
  end;
  function getItem(ind : integer) : T;
  type TPtr = ^T;
  begin
    var tmp := System.IntPtr(beg.ToInt32 + ind * sizeof(T));
    Result := TPtr(tmp.ToPointer)^;
  end;
  
  procedure setItem(ind : integer; it : T);
  begin
    
  end;
  
  property Item[ind : integer] : T read getItem write setItem; default;
  end;

var lst : List<integer>;
begin
lst := new List<integer>;
lst.Add(2);
lst.Add(3);
lst.Add(5);
assert(lst[0] = 2);
assert(lst[1] = 3);
end.