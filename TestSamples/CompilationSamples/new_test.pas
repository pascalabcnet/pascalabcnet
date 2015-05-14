uses vcl;

type 
  myForm=class(Form)
    constructor;
    begin
    end;
  end;{}
 
  
begin
  //writeln(new integer);
  Application.Run(new System.Windows.Forms.Form);
  readln;
end.