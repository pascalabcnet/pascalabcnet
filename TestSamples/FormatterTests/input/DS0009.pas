#reference 'System.Windows.Forms.dll'

type myform=class(System.Windows.Forms.Form)
constructor; begin end;
     end;

begin
  System.Windows.Forms.application.run(new myform);
end.