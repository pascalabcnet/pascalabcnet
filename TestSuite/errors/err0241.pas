type
  t1 = class
    
    function GetP1:byte := 0;
    
    property P1:byte read GetP1; virtual;
    
  end;
  t2 = class(t1)
    
    function NewGetP1:byte;
    begin
      var b := inherited self.P1;//тут вызывается NewGetP1 вместо GetP1
    end;
    
    property P1:byte read NewGetP1; override;
    
  end;

begin
  var b := (new t2).p1;
end.