function f1(b:byte:=2):byte; begin end;
function f2(params arr: array of byte):byte; begin end;
function f3(b: byte): byte; begin end;
function f4(b: byte): byte; begin end;
function f4(params arr: array of byte): byte; begin end;
function f5(a, b: byte): byte; begin end;
begin
  var a{@var a: byte;@} := f1;
  var a2{@var a2: byte;@} := f2;
  var a3{@var a3: function(b: byte): byte;@} := f3;
  var a4{@var a4: byte;@} := f4;
  var a5{@var a5: byte;@} := f5(1);
  var a6{@var a6: byte;@} := f5(1,2,3);
end.