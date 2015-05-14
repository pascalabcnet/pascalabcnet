#apptype windows
#reference 'System.Windows.Forms.dll'

uses System, System.Windows.Forms;
type TClass = class
a : integer;
end;

type TDer = class(TClass)
end;

var myForm, myForm2: Form;
begin
myForm := new System.Windows.Forms.Form;
myForm2 := new Form();
with myForm, Form do
begin
Text := 'Forma';
Left := 23;
CheckForIllegalCrossThreadCalls := false;
Top := 45;
end;

with Console do
begin
  WriteLine('ok');
end;
var t := new TClass;
with t do
begin
  //ToString();
end;
end.