//!Несколько подпрограмм proc1 могут быть вызваны
type
  c1 = class
    // обязательно метод. с подпрограммой не воспроизводится
    procedure method1(act: System.Action);
    begin end;
  end;
  
procedure proc1(par1: System.Action<integer>); begin end;
procedure proc1(par1: System.Action<real>); begin end;

begin
  var var1:= new c1;
  
  var1.method1( 
      ()-> begin
          proc1(par1-> begin end);
      end
  );
  
end.