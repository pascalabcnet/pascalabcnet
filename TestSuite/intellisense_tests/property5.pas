type
  t1 = class
    static property p1[i: integer]: sequence of integer read Arr(0);
  end;
  
begin
  var x{@var x: sequence of integer;@} := t1.p1[0];
  t1.p1[0].Average{@(расширение sequence of T) function Average(): real;@}();
end.