type
  t1<T> = class
  
  public 
    procedure p1; virtual;
    function f1: integer; virtual;
  
  end;

procedure t1<T>.p1{@procedure t1<>.p1(); virtual;@} := p1;
function t1<T>.f1{@function t1<>.f1(): integer; virtual;@}: integer := 1;

begin end.