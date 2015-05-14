#apptype windows
#reference 'System.Windows.Forms.dll'

uses System.Windows.Forms, System.Threading;

var myForm: Form;

procedure RunThread;
begin
  var w := new WebBrowser;
  myForm.Controls.Add(w);
  w.Dock := DockStyle.Fill;
  w.Url := new System.Uri('http://pascalabc.net');
  Application.Run(myForm);
end;

begin
  myForm := new Form;
  myForm.Width := 700;
  myForm.Height := 400;
  var w := new WebBrowser;
  myForm.Controls.Add(w);
  w.Dock := DockStyle.Fill;
  w.Url := new System.Uri('http://pascalabc.net');
  Application.Run(myForm);
  {var th := new Thread(RunThread);
  th.SetApartmentState(System.Threading.ApartmentState.STA);
  th.Start();}
//  myForm.Controls.Add(w);
end.