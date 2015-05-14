Unit
  interface_unit2;

interface
  
uses
  interface_unit;
  
type
  MyC = class(object{, System.ICloneable}, MyInter1)
  public
    constructor;
    begin
    
    end;
    procedure p(x: integer);
    begin
      writeln(x);
    end;
    function Clone: object; virtual;
    begin
      result := new MyC;
    end;
  end;
  
implementation

end.
  
  