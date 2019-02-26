uses GraphWPF;

begin
  Window.Title := 'Цифровые часы';
  Font.Size := 180;
  BeginFrameBasedAnimation(
    ()->DrawText(Window.ClientRect,DateTime.Now.ToLongTimeString.Replace(' AM',''),Colors.Red),
    1
  );
end.