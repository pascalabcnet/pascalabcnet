{$reference PresentationFramework.dll}
{$apptype windows}
var i: integer;

type
  t1 = class(System.Windows.Window)
    
    procedure p1;
    begin
      var p := procedure->
      begin
        Dispatcher.Invoke(()->begin Inc(i) end);
      end;
      p;
    end;
    
  end;
  
begin 
  var o := new t1;
  o.p1;
  assert(i = 1);
end.