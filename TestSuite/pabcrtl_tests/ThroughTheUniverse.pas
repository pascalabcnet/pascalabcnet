//Программа "Скозь вселенную". Порт с midletPascal

uses GraphABC;
 
 type
 // Описываем тип-элемент Звезда
   TStar = record
     X, Y, Z : real; // Положение в пространстве
   end;

 const
   MAX_STARS = 1000;        // Кол-во звёздочек
   SPEED = 200;             // Скорость, в единицах/сек

 var
   i     : Integer;
 // Наши звёздочки :)  
   Stars : array [1..MAX_STARS] of TStar;
 // Ширина и высота дисплея
   scr_W : Integer;
   scr_H : Integer;
 // Время
   time, dt : Integer;

 // Рисует текущую звёздочку (i), цвета (c)
   procedure SetPix(c: Integer);
   var
     sx, sy : Integer;
   begin
   // Данные действия, проецируют 3D точку на 2D полоскость дисплея
   try
     sx := trunc(scr_W / 2 + Stars[i].X * 200 / (Stars[i].Z + 200));
     sy := trunc(scr_H / 2 - Stars[i].Y * 200 / (Stars[i].Z + 200));
     except
     end;
     
     try
      SetPixel(sx, sy, Color.FromArgb(c, c, c));
     except
     end;
   end;

 begin
   MaximizeWindow();
   scr_W := Window.Width;  
   scr_H := Window.Height;

 //случайным образом раскидаем звёздочки
   randomize;
   for i := 1 to MAX_STARS do
   begin
     Stars[i].X := random(scr_W * 4) - scr_W * 2;
     Stars[i].Y := random(scr_H * 4) - scr_H * 2;
     Stars[i].Z := random(1900);
   end;
   
 // Очистка содержимого дисплея (чёрный цвет)  
   ClearWindow(Color.Black); 
   
   time := Milliseconds;
 // Главный цикл отрисовки
   repeat
     scr_W := Window.Width;  
     scr_H := Window.Height;
     dt   := Milliseconds - time;  // Сколько мс прошло, с прошлой отрисовки
     time := Milliseconds;         // Засекаем время
     for i := 1 to MAX_STARS do
       begin
     // Затираем звёздочку с предыдущего кадра
       SetPix(0);
     // Изменяем её позицию в зависимости прошедшего с последней отрисовки времени
       Stars[i].Z := Stars[i].Z - SPEED * dt/1000;
     // Если звезда "улетела" за позицию камеры - генерируем её вдали
       if Stars[i].Z <= -200 then
       begin
         Stars[i].X := random(scr_W * 4) - scr_W * 2;
         Stars[i].Y := random(scr_H * 4) - scr_H * 2;
         Stars[i].Z := 1900; // Откидываем звезду далеко вперёд :)
       end;
     // Рисуем звёздочку в новом положении (цвет зависит от Z координаты) 
       SetPix(trunc(255 - 255 * (Stars[i].Z + 200) / 2100));
     end;
     sleep(10);
   until false;
 end.