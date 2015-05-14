{$reference 'System.Windows.Forms.dll'}
{$apptype windows}

uses System.Windows.Forms;

var myForm: Form;
    myButton: Button;
    myTextBox: TextBox;
begin
  myForm := new Form;
  myButton := new Button;
  myTextBox := new TextBox;
  myButton.Text := 'Кнопка1';
  myButton.Left := 50;
  myButton.Top := 80;
  myTextBox.Text := 'Текст';
  myForm.Controls.Add(myButton);
  myForm.Controls.Add(myTextBox);
    readln;
  Application.Run(myForm);
end.
