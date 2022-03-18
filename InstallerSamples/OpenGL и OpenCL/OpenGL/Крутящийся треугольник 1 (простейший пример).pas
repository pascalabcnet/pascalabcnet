{$reference System.Windows.Forms.dll}
{$reference System.Drawing.dll}

// Данный пример демонстрирует запуск простейшей программы с модулем OpenGL
// Примите во внимание, что методы из gl_gdi. - могут служить только временной заменой в серьёзной программе
// (По крайней мере в данной версии. В будущем, возможно, gl_gdi будет улучшено)
// Они использованы тут, чтоб пример был проще

uses System.Windows.Forms;
uses System.Drawing;
uses System;
uses OpenGL;

{$apptype windows} // убирает консоль

var gl: OpenGL.gl<PlWin>;

{$region Shader}

function InitShader(fname: string; st: ShaderType): gl_shader;
begin
  Result := gl.CreateShader(st);
  
  var source := ReadAllText(fname);
  
  gl.ShaderSource(Result, 1,
    new string[](source),
    nil
  );
  
  gl.CompileShader(Result);
  // получаем состояние успешности компиляции
  // 1=успешно
  // 0=ошибка
  var comp_ok: integer;
  gl.GetShaderiv(Result, ShaderParameterName.COMPILE_STATUS, comp_ok);
  if comp_ok = 0 then
  begin
    
    // узнаём нужную длинную строки
    var l: integer;
    gl.GetShaderiv(Result, ShaderParameterName.INFO_LOG_LENGTH, l);
    
    // выделяем достаточно памяти чтоб сохранить строку
    var ptr := System.Runtime.InteropServices.Marshal.AllocHGlobal(l);
    
    // получаем строку логов
    gl.GetShaderInfoLog(Result, l, IntPtr.Zero, ptr);
    
    // преобразовываем в управляемую строку
    var log := System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
    Writeln(log);
    
    // и в конце обязательно освобождаем, чтобы не было утечек памяти
    System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
    gl.DeleteShader(Result);
    
    Result := gl_shader.Zero;
  end;
  
end;

{$endregion Shader}

{$region Program}

function InitProgram(vertex_shader, fragment_shader: gl_shader): gl_program;
begin
  Result := gl.CreateProgram;
  
  gl.AttachShader(Result, vertex_shader);
  if fragment_shader<>gl_shader.Zero then gl.AttachShader(Result, fragment_shader);
  
  gl.LinkProgram(Result);
  // всё то же самое что и у шейдеров
  var link_ok: integer;
  gl.GetProgramiv(Result, ProgramPropertyARB.LINK_STATUS, link_ok);
  if link_ok = 0 then
  begin
    
    var l: integer;
    gl.GetProgramiv(Result, ProgramPropertyARB.INFO_LOG_LENGTH, l);
    var ptr := System.Runtime.InteropServices.Marshal.AllocHGlobal(l);
    
    gl.GetProgramInfoLog(Result, l, IntPtr.Zero, ptr);
    var log := System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
    Writeln(log);
    
    System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
    gl.DeleteProgram(Result);
    
    Result := gl_program.Zero;
  end;
  
end;

{$endregion Program}

const dy = -Sin(Pi / 6) / 2;

begin
  
  // Создаёт и настраиваем окно
  var f := new Form;
  f.StartPosition := FormStartPosition.CenterScreen;
  f.ClientSize := new Size(500, 500);
  f.FormBorderStyle := FormBorderStyle.Fixed3D;
  // Если окно закрылось - надо сразу завершить программу
  f.Closed += (o,e)->Halt();
  
  // Настраиваем поверхность рисования
  var hdc := gl_gdi.InitControl(f);
  
  // Настраиваем перерисовку
  gl_gdi.SetupControlRedrawing(f, hdc, EndFrame->
  begin
    
    {$region Настройка глобальных параметров OpenGL}
    
    // При создании экземпляра OpenGL.gl инициализируются некоторые функции
    // Это необходимо для всех функций из OpenGL1.2 и выше, потому что они локальны для контекста OpenGL
    gl := new OpenGL.gl<PlWin>;
    
    {$endregion Настройка глобальных параметров OpenGL}
    
    {$region Инициализация переменных}
    
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
    
    var vertex_clr_buffer: gl_buffer;
    gl.CreateBuffers(1, vertex_clr_buffer);
    gl.NamedBufferData(
      vertex_clr_buffer,
      new IntPtr(3*sizeof(Vec3f)),
      new Vec3f[](
        new Vec3f(1,0,0),
        new Vec3f(0,1,0),
        new Vec3f(0,0,1)
      ),
      VertexBufferObjectUsage.STATIC_DRAW
    );
    
    var element_buffer: gl_buffer;
    gl.CreateBuffers(1, element_buffer);
    gl.NamedBufferData(
      element_buffer,
      new IntPtr(3*sizeof(byte)),
      new byte[](
        0,1,2
      ),
      VertexBufferObjectUsage.STATIC_DRAW
    );
    
    var vertex_shader := InitShader('Rot Triangle 1.vertex.glsl', ShaderType.VERTEX_SHADER);
//    var fragment_shader := InitShader('fragment.glsl', ShaderType.FRAGMENT_SHADER);
    
    var sprog := InitProgram(vertex_shader, gl_shader.Zero {fragment_shader});
    
    var uniform_rot_k :=     gl.GetUniformLocation(sprog, 'rot_k');
    
    var attribute_position := gl.GetAttribLocation(sprog, 'position');
    var attribute_color :=    gl.GetAttribLocation(sprog, 'color');
    
    var t := new Stopwatch;
    t.Start;
    
    {$endregion Инициализация переменных}
    
    while true do
    begin
      // очищаем окно в начале перерисовки
      gl.Clear(ClearBufferMask.COLOR_BUFFER_BIT);
      
      
      
      gl.UseProgram(sprog);
      
      gl.Uniform1f(uniform_rot_k, Cos( t.Elapsed.Ticks * 0.0000002 ) );
      
      gl.BindBuffer(BufferTargetARB.ARRAY_BUFFER, vertex_pos_buffer);
      gl.VertexAttribPointer(
        attribute_position,
        2,
        VertexAttribPointerType.FLOAT,
        false,
        8,
        IntPtr.Zero
      );
      gl.EnableVertexAttribArray(attribute_position);
      
      gl.BindBuffer(BufferTargetARB.ARRAY_BUFFER, vertex_clr_buffer);
      gl.VertexAttribPointer(
        attribute_color,
        3,
        VertexAttribPointerType.FLOAT,
        false,
        12,
        IntPtr.Zero
      );
      gl.EnableVertexAttribArray(attribute_color);
      
      gl.BindBuffer(BufferTargetARB.ELEMENT_ARRAY_BUFFER, element_buffer);
      gl.DrawElements(
        PrimitiveType.TRIANGLES,
        3,
        DrawElementsType.UNSIGNED_BYTE,
        IntPtr.Zero
      );
      
      gl.DisableVertexAttribArray(attribute_position);
      gl.DisableVertexAttribArray(attribute_color);
      
      
      
      // получаем тип последней ошибки
      var err := gl.GetError;
      // и если ошибка есть - выводим её
      if err<>ErrorCode.NO_ERROR then Writeln(err);
      
      gl.Finish;
      // EndFrame меняет местами буферы и ждёт vsync
      EndFrame;
    end;
    
  end);
  
  Application.Run(f);
end.