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

{$apptype windows} // убираем консоль

var gl: OpenGL.gl;

{$region Buffer}

function InitBuffer(sz: integer; data: IntPtr): BufferName;
begin
  gl.CreateBuffers(1, Result);
  
  gl.NamedBufferData(Result, new UIntPtr(sz), data, BufferDataUsage.STATIC_DRAW);
  
end;

function InitBuffer(sz: integer; data: pointer) := InitBuffer(sz, IntPtr(data));
function InitBuffer<T>(sz: integer; var data: T) := InitBuffer(sz, @data);
function InitBuffer<T>(data: array of T) := InitBuffer(data.Length*System.Runtime.InteropServices.Marshal.SizeOf&<T>, data[0]);

{$endregion Buffer}

{$region Shader}

function InitShader(fname: string; st: ShaderType): ShaderName;
begin
  Result := gl.CreateShader(st);
  
  var source := ReadAllText(fname);
  
  // в данной версии модуля OpenGL параметры принимающие массив строк - не поддерживаются
  // поэтому надо ручками преобразовывать управляемую строку в неуправляемую с кодировкой ANSI 
  var source_strptr := System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(source);
  var source_len := source.Length;
  
  gl.ShaderSource(Result, 1, source_strptr, source_len);
  
  // и обязательно освобождаем память, иначе снова утечка памяти
  System.Runtime.InteropServices.Marshal.FreeHGlobal(source_strptr);
  
  gl.CompileShader(Result);
  // получаем состояние успешности компиляции
  // 1=успешно
  // 0=ошибка
  var comp_ok: integer;
  gl.GetShaderiv(Result, ShaderInfoType.COMPILE_STATUS, comp_ok);
  if comp_ok <> 1 then
  begin
    
    // узнаём нужную длинную строки
    var l: integer;
    gl.GetShaderiv(Result, ShaderInfoType.INFO_LOG_LENGTH, l);
    
    // выделяем достаточно памяти чтоб сохранить строку
    var ptr := System.Runtime.InteropServices.Marshal.AllocHGlobal(l);
    
    // получаем строку логов
    gl.GetShaderInfoLog(Result, l, nil, ptr);
    
    // преобразовываем в управляемую строку
    var log := System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
    writeln(log);
    
    // и опять же, в конце обязательно освобождаем памяти, чтоб не было утечек памяти
    System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
  end;
  
end;

{$endregion Shader}

{$region Program}

function InitProgram(vertex_shader, fragment_shader: ShaderName): ProgramName;
begin
  Result := gl.CreateProgram;
  
  gl.AttachShader(Result, vertex_shader);
  if fragment_shader<>0 then gl.AttachShader(Result, fragment_shader);
  
  gl.LinkProgram(Result);
  // всё то же самое что и у шейдеров
  var link_ok: integer;
  gl.GetProgramiv(Result, ProgramInfoType.LINK_STATUS, link_ok);
  if link_ok <> 1 then
  begin
    
    var l: integer;
    gl.GetProgramiv(Result, ProgramInfoType.INFO_LOG_LENGTH, l);
    var ptr := System.Runtime.InteropServices.Marshal.AllocHGlobal(l);
    
    gl.GetProgramInfoLog(Result, l, nil, ptr);
    var log := System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
    writeln(log);
    
    System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
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
    
    // при создании экземпляра OpenGL.gl инициализируются некоторые функции
    // это необходимо для всех функций из OpenGL1.2 и выше, потому что они локальные для контекста OpenGL
    gl := new OpenGL.gl;
    
    {$endregion Настройка глобальных параметров OpenGL}
    
    {$region Инициализация переменных}
    
    var vertex_pos_buffer := InitBuffer(ArrGen(3, i->
    begin
      var rot := i * Pi * 2 / 3;
      
      Result := new Vec2f(
        Sin(rot),
        Cos(rot) + dy
      );
      
    end));
    var vertex_clr_buffer := InitBuffer(new Vec3f[](
      new Vec3f(1,0,0),
      new Vec3f(0,1,0),
      new Vec3f(0,0,1)
    ));
    
    var element_buffer := InitBuffer(new byte[](
      0,1,2
    ));
    
    var vertex_shader := InitShader('Rot Triangle 1.vertex.glsl', ShaderType.VERTEX_SHADER);
//    var fragment_shader := InitShader('fragment.glsl', ShaderType.FRAGMENT_SHADER);
    
    var sprog := InitProgram(vertex_shader, 0 {fragment_shader});
    
    var uniform_rot_k :=      gl.GetUniformLocation(sprog, 'rot_k');
    var attribute_position := gl.GetAttribLocation(sprog, 'position');
    var attribute_color :=    gl.GetAttribLocation(sprog, 'color');
    
    var t := new System.Diagnostics.Stopwatch;
    t.Start;
    
    {$endregion Инициализация переменных}
    
    while true do
    begin
      // очищаем окно в начале перерисовки
      gl.Clear(BufferTypeFlags.COLOR_BUFFER_BIT);
      
      
      
      gl.UseProgram(sprog);
      
      gl.Uniform1f(uniform_rot_k, Cos( t.Elapsed.Ticks * 0.0000002 ) );
      
      gl.BindBuffer(BufferBindType.ARRAY_BUFFER, vertex_pos_buffer);
      gl.VertexAttribPointer(
        attribute_position,
        2,
        DataType.FLOAT,
        false,
        8,
        nil
      );
      gl.EnableVertexAttribArray(attribute_position);
      
      gl.BindBuffer(BufferBindType.ARRAY_BUFFER, vertex_clr_buffer);
      gl.VertexAttribPointer(
        attribute_color,
        3,
        DataType.FLOAT,
        false,
        12,
        nil
      );
      gl.EnableVertexAttribArray(attribute_color);
      
      gl.BindBuffer(BufferBindType.ELEMENT_ARRAY_BUFFER, element_buffer);
      gl.DrawElements(
        PrimitiveType.TRIANGLES,
        3,
        DataType.UNSIGNED_BYTE,
        pointer(nil)
      );
      
      gl.DisableVertexAttribArray(attribute_position);
      gl.DisableVertexAttribArray(attribute_color);
      
      
      
      // получаем тип последней ошибки
      var err := gl.GetError;
      // и если ошибка есть - выводим её
      if err.val<>ErrorCode.NO_ERROR then writeln(err);
      
      gl.Finish;
      // EndFrame меняет местами буферы и ждёт vsync
      EndFrame;
    end;
    
  end);
  
  Application.Run(f);
end.