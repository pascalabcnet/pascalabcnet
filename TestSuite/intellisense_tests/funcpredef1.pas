type 
c = class
  static procedure pr1(param1: integer:= 0);
  static function f(param1: integer := 1): integer;
end;

static procedure c.pr1(param1: integer); begin end;
static function c.f(param1: integer): integer;
begin
  
end;
begin
c.pr1{@static procedure c.pr1(param1: integer:=0);@};
c.f{@static function c.f(param1: integer:=1): integer;@}();
end.