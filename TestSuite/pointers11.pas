type
  t0<T> = class end;
  t1 = class(t0<byte>)
    
    ptr: ^byte;
    ptr2: ^string[5];
    procedure p1;
    begin
      New(ptr);
      ptr^ := 2;
      New(ptr2);
      ptr2^ := 'abcdef';
    end;
    
  end;

begin 
  var o := new t1;
  o.p1;
  assert(o.ptr^ = 2);
  assert(o.ptr2^ = 'abcde');
  dispose(o.ptr);
  dispose(o.ptr2);
end.