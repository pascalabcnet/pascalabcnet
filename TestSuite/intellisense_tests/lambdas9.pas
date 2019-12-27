begin
  var a := ArrGen(
    1,
    i ->
    begin
      Result{@var Result: Object;@} := new object;
    end
  );
end.