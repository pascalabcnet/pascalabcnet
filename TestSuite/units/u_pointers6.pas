unit u_pointers6;

type TRec = record
    b1, b2, b3, b4, b5, b6, b7, b8: byte;
  end;

var rec : TRec;
    r : real := 3.14;
    prec : ^TRec;
    pi : ^int64;
    i : int64;
    pr : ^real;
    ptr : pointer;
    iptrptr : ^^integer;
    iptr : ^integer;
    i2 : integer;
    
begin
  prec := pointer(@r);
  rec := prec^;
  pi := pointer(@r);
  i := pi^;
  pr := pointer(pi);
  assert(pr^=3.14);
  i2 := 7;
  iptr := @i2;
  ptr := iptr;
  iptr := ptr;
  ptr := pointer(iptr);
  iptr := PInteger(ptr);
  iptr := pinteger(pointer(iptrptr));
  iptrptr := ptr;
end.