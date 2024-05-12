var p :=  4;

type 
  a = class 
    p := 4;   
    property prop : Integer read p;
    procedure outer1();
  end;
  
  b = class 
    procedure test();
    begin
      p := 3;
    end;
    
    procedure test1();
    var p := 5;
    begin
      var k := p;  
    end;
   procedure outer2();
    end;
    
   c = class(a)
    procedure t();
    begin
      var c := p;
    end;
    
    procedure outer3();
    
    end;
    
 procedure a.outer1();
 begin
    Print(prop);
    Print(p);
 end;
 
 procedure b.outer2();
 begin
     Print(p);
 end;
  
  procedure c.outer3();
 begin
     Print(p);
 end;
 
 begin
   Print(p);
 end.