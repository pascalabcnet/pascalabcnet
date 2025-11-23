type
  t1<T> = class
  
  public 
    procedure p1; virtual;
  
  end;

procedure t1<T>.p1{@procedure t1<>.p1(); virtual;@} := p1;

begin end.