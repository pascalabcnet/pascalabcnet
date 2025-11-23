type
  I0<T> = interface end;
  I1<T> = interface(I0<I1<I0<T>>>) end;

begin end.