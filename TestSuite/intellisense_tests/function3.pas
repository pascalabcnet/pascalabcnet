type
  TClass = class
    function f(params arr: array of char): char;
    begin
      
    end;
    
    function f2(a: integer := 2): integer;
    begin
      
    end;
  end;

begin
  var a: sequence of byte;
  var b{@var b: array of byte;@} := a.ToArray;
  var o := new TClass;
  var c{@var c: char;@} := o.f;
  var i{@var i: integer;@} := o.f2;
end.