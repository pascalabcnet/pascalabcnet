begin
  var arr1 := Arr(1.2,2.3,3.2);
  var r{@var r: real;@} := arr1.Sum{@(расширение sequence of T) function Sum(): real;@}();
  var arr2 := Arr(1,2,3);
  var i{@var i: integer;@} := arr2.Sum{@(расширение sequence of T) function Sum(): integer;@}();
end.