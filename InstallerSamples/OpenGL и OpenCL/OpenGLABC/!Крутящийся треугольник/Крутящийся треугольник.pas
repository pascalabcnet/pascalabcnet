{$reference System.Windows.Forms.dll}
{$reference System.Drawing.dll}

// Данный пример демонстрирует запуск простейшей программы с модулем OpenGL
// Примите во внимание, что модуль OpenGLABC будет сильно переделан
// Его текущее содержимое может служить только временной заменой в серьёзной программе
// Он использованы тут, чтобы пример был проще

uses System.Windows.Forms;
uses System;
uses OpenGL;
uses OpenGLABC;

uses Common in '..\Common';

{$apptype windows} // убираем консоль

const dy = -Sin(Pi / 6) / 2;

procedure RedrawProc(pl: PlatformLoader; EndFrame: ()->());
begin
  
  {$region Настройка глобальных параметров OpenGL}
  
  // При создании экземпляра OpenGL.gl в память загружаются адреса функций OpenGL
  // Это необходимо для функций из OpenGL, потому что они локальны для контекста OpenGL
  // Переменная pl указывает как надо загружать адреса
  gl := new OpenGL.gl(pl);
  
  {$endregion Настройка глобальных параметров OpenGL}
  
  {$region Инициализация переменных}
  
  var sprog := InitProgram(
    InitShader('Крутящийся треугольник.vert', ShaderType.VERTEX_SHADER)
  {, другие_шейдеры});
  gl.UseProgram(sprog);
  
  var uniform_rot_k :=     gl.GetUniformLocation(sprog, 'rot_k');
  
  // Значения, которые будут передаваться в атрибут "position" в шейдере
  begin
    var vertex_pos_buffer: gl_buffer;
    gl.CreateBuffers(1, vertex_pos_buffer);
    gl.NamedBufferData(
      vertex_pos_buffer,
      new IntPtr(3*sizeof(Vec2f)),
      ArrGen(3, i->
      begin
        var rot := i * Pi * 2 / 3;
        
        Result := new Vec2f(
          Sin(rot),
          Cos(rot) + dy
        );
        
      end),
      VertexBufferObjectUsage.STATIC_DRAW
    );
    var attribute_position := gl.GetAttribLocation(sprog, 'position');
    // Оставляем всё настроенным и привязанным
    // В данном случае можно, потому что шейдерная программа одна
    gl.VertexAttribFormat(attribute_position, 2,VertexAttribType.FLOAT, false, 0);
    gl.BindVertexBuffer(attribute_position, vertex_pos_buffer, IntPtr.Zero, sizeof(Vec2f));
    gl.EnableVertexAttribArray(attribute_position);
  end;
  
  // Значения, которые будут передаваться в атрибут "color" в шейдере
  begin
    var vertex_clr_buffer: gl_buffer;
    gl.CreateBuffers(1, vertex_clr_buffer);
    gl.NamedBufferData(
      vertex_clr_buffer,
      new IntPtr(3*sizeof(Vec3f)),
      |
        new Vec3f(1,0,0),
        new Vec3f(0,1,0),
        new Vec3f(0,0,1)
      |,
      VertexBufferObjectUsage.STATIC_DRAW
    );
    var attribute_color := gl.GetAttribLocation(sprog, 'color');
    gl.VertexAttribFormat(attribute_color, 3,VertexAttribType.FLOAT, false, 0);
    gl.BindVertexBuffer(attribute_color, vertex_clr_buffer, IntPtr.Zero, sizeof(Vec3f));
    gl.EnableVertexAttribArray(attribute_color);
  end;
  
  var t := new Stopwatch;
  t.Start;
  
  {$endregion Инициализация переменных}
  
  // Для поиска, где возникла ошибка
//  var RaiseIfError := procedure->
//  begin
//    
//    // получаем тип последней ошибки
//    var err := gl.GetError;
//    // и если ошибка есть - выводим её
//    if err<>ErrorCode.NO_ERROR then raise new System.Exception(err.ToString);
//    
//  end;
  
  while true do
  begin
    // очищаем окно в начале перерисовки
    gl.Clear(ClearBufferMask.COLOR_BUFFER_BIT);
    
    
    
    gl.Uniform1f(uniform_rot_k, Cos( t.Elapsed.Ticks * 0.0000002 ) );
    
    // Рисуем треугольники из всего 3 вершин (то есть получится 1 треугольник)
    // Для каждой вершины берём 1 значение из vertex_pos_buffer и 1 из vertex_clr_buffer
    gl.DrawArrays(PrimitiveType.TRIANGLES, 0,3);
    
    
    
    // получаем тип последней ошибки
    var err := gl.GetError;
    // и если ошибка есть - выводим её
    if err<>ErrorCode.NO_ERROR then Writeln(err);
    
    gl.Finish;
    // EndFrame меняет местами буферы и ждёт vsync
    EndFrame;
  end;
  
end;

begin
  
  // Создаём и настраиваем окно
  var f := new Form;
  f.StartPosition := FormStartPosition.CenterScreen;
  f.ClientSize := new System.Drawing.Size(500, 500);
  f.FormBorderStyle := FormBorderStyle.Fixed3D;
  // Если окно закрылось - надо сразу завершить программу
  // Иначе поток перерисовки продолжит пытаться рисовать на закрытом окне
  f.Closed += (o,e)->Halt();
  
  // Настраиваем поверхность рисования
  var hdc := OpenGLABC.gl_gdi.InitControl(f);
  
  // Настраиваем перерисовку
  f.Load += (o,e)->OpenGLABC.RedrawHelper.SetupRedrawThread(hdc, RedrawProc);
  
  Application.Run(f);
end.