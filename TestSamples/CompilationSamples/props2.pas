//Тест базовой работоспособности интерфейсов
type
  inter = interface
    function getnn: integer;
    procedure setnn(value: integer);
    property nn: integer read getnn write setnn;
  end;

  inter2 = interface(inter, System.ICloneable)
    procedure pr_1(s: string);
    procedure pr_2(s: string);
    function udv_1(r: integer): integer;
    function udv_2(r: integer): integer;
  end;
  
  base_class = class
  public
    procedure pr_1(s: string);
    begin
      writeln(s);
    end;
    procedure pr_2(s: string); virtual;
    begin
      writeln(s);
    end;
    function udv_1(r: integer): integer;
    begin
      result := r*2;
    end;
    function udv_2(r: integer): integer; virtual;
    begin
      result := r*2;
    end;
  end;
  
  c = class(base_class, inter2)
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
    
    //function udv_1(r: integer): integer;
    //begin
    //  result := inherited udv_1(r);
    //end;

    procedure say;
    function Clone: object;
    begin
      result := new c;
    end;
    property nn: integer read getnn write setnn;
  end;

procedure c.say;
begin
  writeln('Hello!');
end;

var
  bcl: base_class;
  obj, obj2: c;
  f: inter;
  f2: inter2;
  iclon: System.ICloneable;
  
begin
  bcl := new base_class;
  writeln(bcl.udv_1(1));
  obj := new c;
  writeln(inter(obj));
  f := obj;
  writeln(f.nn);
  obj2 := c(f);
  obj2.say;
  iclon := obj;
  writeln(iclon.Clone);
  f2 := obj;
  f2.pr_1('eeeeee');
  f2.pr_2('fffffffffff');
  writeln(f2.udv_1(3));
  writeln(f2.udv_2(7));
  readln;
end.