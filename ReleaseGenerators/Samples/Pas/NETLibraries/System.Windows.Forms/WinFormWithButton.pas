// Создание оконного приложения
{$apptype windows}
{$reference 'System.Windows.Forms.dll'}

uses 
  System,
  System.Windows.Forms;

var 
  myForm: Form;
  myButton: Button;
  
procedure MyButtonClick(sender: Object; e: EventArgs);
begin
  myForm.Close;
end;

begin
  myForm := new Form;
  myForm.Text := 'Оконное приложение';
  myButton := new Button;
  myButton.Text := '  Закрыть окно  ';
  myButton.AutoSize := True;
  myButton.Left := 90;
  myButton.Top := 110;
  myForm.Controls.Add(myButton);
  myButton.Click += MyButtonClick;
  Application.Run(myForm);
end.
