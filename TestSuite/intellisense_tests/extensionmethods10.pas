begin
  var f: file of integer;
  f.Read{@(расширение file of T) function PABCExtensions.Read<T>(): T;@}();
  var f2: file;
  f2.Read{@@}();
end.