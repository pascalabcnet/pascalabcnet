{$reference 'System.Windows.Forms.dll'}
{$reference 'System.Drawing.dll'}
uses VCL, System, System.Collections, System.Windows.Forms, System.Drawing;

procedure del(oo: System.Object; ea: System.EventArgs);
begin
  MessageBox.Show('Button clicked');
  MessageBox.Show('My delegate called!!!');
end;

type
  TMyForm = class(Form)
    btn1: Button;
    txtBox1: TextBox;
    tstBar : MainMenu;
    
    procedure Test;
    begin
    ActivateMdiChild(self);var k := Form.ScrollStateAutoScrolling;
    end;
    
    constructor Create;
    var d : integer;
    begin
      Height := 300;
      Width := 400;
      
      Opacity := 0.6;
      Text := 'SuperForm';
      BackColor := Color.FromArgb(255, 100, 200, 100);
      btn1 := Button.Create;
      btn1.Left := 200;
      btn1.Top := 200;
      btn1.Height := 30;
      btn1.Width := 80;
      btn1.Text := 'Click me';
      btn1.add_Click(del);
      txtBox1 := TextBox.Create;
      txtBox1.Left := 10;
      txtBox1.Top := 10;
      txtBox1.Height := 30;
      d := txtBox1.Height;
      writeln(d);
      txtBox1.Width := 200;
      txtBox1.Text := 'Enter text here';
      Controls.Add(btn1);
      Controls.Add(txtBox1);
    end;
  end;

var
  f: Form;
  ff: TMyForm;
  b: boolean;

begin
  f := Form.Create;
  b := f.Modal;
  f.Width := 600;
  f.Controls.Add(Button.Create);
  Console.WriteLine(b);
  Application.Run(TMyForm.Create);
  MessageBox.Show('Application exit');
end.