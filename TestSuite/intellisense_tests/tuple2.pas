function f1:(byte,byte);
begin
  
end;

function f2:(one,two);
begin
  
end;

begin
  var t := f1{@function f1(): (byte,byte);@};
  var b{@var b: byte;@} := t.Item1;
  var t1 := f2;
  var i{@var i;@} := t1.Item1;
  var j{@var j;@} := i;
end.