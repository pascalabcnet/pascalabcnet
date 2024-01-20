//!Нельзя реализовывать метод из другого модуля
unit err0534;
interface

uses err0533;

function f0: integer;

implementation

static function t1.f1 := 2;

function f0 := t1.f1;

end.