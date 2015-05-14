Unit simple_class_unit;

interface

type
  MyClass = class
  public
    procedure qqq;
    begin
      writeln('qqq');
    end;
  	constructor Create;
  end;
  
implementation

constructor MyClass.Create;
begin
  //inherited Create;
end;

end.