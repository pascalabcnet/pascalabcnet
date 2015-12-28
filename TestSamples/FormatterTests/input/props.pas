//Тест базовой работоспособности интерфейсов
type
  inter = interface
    function getnn: integer;
    procedure setnn(value: integer);
    property nn: integer read getnn write setnn;
  end;

  inter2 = interface(inter, System.ICloneable)
  end;

  c = class(inter2)
  public
    constructor;
    begin
    
    end;
    function getnn: integer; virtual;
    begin
      result := 3;
    end;
    procedure setnn(value: integer); virtual;
    begin
    
    end;
    function Clone: object;
    begin
      result := new c;
    end;
    procedure say;
    //property nn: integer read getnn write setnn;
  end;

procedure c.say;
begin
  writeln('Hello!');
end;

var
  obj, obj2: c;
  f: inter;
  iclon: System.ICloneable;
  
begin
  obj := new c;
  writeln(inter(obj));
  f := obj;
  writeln(f.nn);
  obj2 := c(f);
  obj2.say;
  iclon := obj;
  writeln(iclon.Clone);
  readln;
end.