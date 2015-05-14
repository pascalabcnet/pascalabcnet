unit MathForKumir;
interface
 function ctg(x:real):real;
 function arcctg(x:real):real;
 function md (x,y:integer):integer;
 function dv(x,y:integer):integer;
 function Power(x,y:real):real;

implementation
  function ctg(x:real):real;
  begin
    result:=1/tan(x);
  end;
  function arcctg(x:real):real;
  begin
    result:=Pi/2-arctan(x);;
  end;
  function md (x,y:integer):integer;
  begin
    result:=x mod y;
    
  end;
  function dv(x,y:integer):integer;
  begin
    result:=x div y;
  end;
   function Power(x,y:real):real;
   begin
    result:=Exp(y*Ln(x));
   end;
   
end.