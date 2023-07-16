//!Совместное использование модификаторов override и reintroduce недопустимо
type
  c1 = abstract class
    procedure p1; abstract;
  end;

  c2 = class (c1)
    procedure p1; override;reintroduce;   
    begin end;
  end;

begin end.