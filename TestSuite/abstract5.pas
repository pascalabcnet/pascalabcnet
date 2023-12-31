{$reference PresentationFramework.dll}
{$apptype windows}
uses System.Windows.Controls;

type
  //Ошибка: Класс MyTextBox абстрактный и не может иметь атрибут sealed, потому что метод CreateRenderScope не реализован
  MyTextBox = sealed class(RichTextBox)
    
  end;
  
begin 
  new MyTextBox();
end.