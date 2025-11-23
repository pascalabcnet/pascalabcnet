uses System;

type
  Enum = (A, B);

begin
  var arr := new ValueTuple<Enum, Enum>[](new ValueTuple<Enum, Enum>(A,B));
  assert(arr[0].Item1 = A);
  assert(arr[0].Item2 = B);
end.