type
  a = class
    static function fun(x: integer): integer;
    begin end;
  end;

begin
  var p{@var p: function(x: integer): integer;@}:= a.fun;
end.