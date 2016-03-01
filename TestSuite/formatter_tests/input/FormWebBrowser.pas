// Иллюстрация использования компонента WebBrowser
{$apptype windows}
{$reference 'System.Windows.Forms.dll'}

uses 
  System.Windows.Forms,
  System.Net;

begin
  var myForm := new Form; 
  var w := new WebBrowser;
  w.Url := new System.Uri('http://pascalabc.net');
  w.Dock := Dockstyle.Fill; 
  myForm.Controls.Add(w);
  myForm.WindowState := FormWindowState.Maximized; 
  Application.Run(myForm);
end.
