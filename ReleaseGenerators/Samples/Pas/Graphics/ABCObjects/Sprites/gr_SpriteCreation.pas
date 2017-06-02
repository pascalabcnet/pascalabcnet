// Создание спрайта и его состояний
uses GraphABC,ABCSprites,ABCObjects,Events;

var s: SpriteABC;

begin
  Window.Title := 'Создание спрайта';
  SetWindowSize(400,300);
  CenterWindow;
  
  // Создание спрайта и добавление в него кадров
  s := new SpriteABC(150,100,'SpriteFrames\multi1.bmp');
  s.Add('SpriteFrames\multi2.bmp');
  s.Add('SpriteFrames\multi3.bmp');
  s.Add('SpriteFrames\multi2.bmp');
  s.Add('SpriteFrames\multi4.bmp');
  s.Add('SpriteFrames\multi5.bmp');
  
  // Добавление состояний к спрайту
  s.AddState('fly',4); // Летать - 4 кадра
  s.AddState('stand',1); // Стоять - 1 кадр
  s.AddState('sit',1); // Сидеть - 1 кадр
  
  // Задание скорости спрайт-анимации (1..10)
  s.Speed := 9;
  
  // Сохранение спрайта в "длинный" рисунок и создание информационного файла спрайта
  s.SaveWithInfo('spr.png');
end.