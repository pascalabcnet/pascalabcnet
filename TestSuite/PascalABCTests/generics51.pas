type
  I1<T>=interface end;
  C1<T>=class(I1<T>) end;
  C2=class(C1<integer>) end;

procedure p1<T,T1>(self: sequence of T; l: T->T1; o: I1<T1>); extensionmethod;
begin
  assert(typeof(T) = typeof(byte));
  assert(typeof(T1) = typeof(integer));
end;
procedure p2<T,T1>(self: sequence of T; o: I1<T1>); extensionmethod := exit;

begin
  // любая 1 из этих строк даёт ошибку:
  Seq&<byte>.p1(b->0,new C2);
  Seq&<byte>.p2(new C2);
end.