unit Common;

uses System;
uses OpenGL;

var gl: OpenGL.gl;

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
  // Получаем состояние успешности компиляции
  // 1=успешно
  // 0=ошибка
  var comp_ok: integer;
  gl.GetShaderiv(Result, ShaderParameterName.COMPILE_STATUS, comp_ok);
  if comp_ok = 0 then
  begin
    
    // Узнаём нужную длинную строки
    var l: integer;
    gl.GetShaderiv(Result, ShaderParameterName.INFO_LOG_LENGTH, l);
    
    // Выделяем достаточно памяти чтоб сохранить строку
    var ptr := System.Runtime.InteropServices.Marshal.AllocHGlobal(l);
    
    // Получаем строку логов
    gl.GetShaderInfoLog(Result, l, IntPtr.Zero, ptr);
    
    // Преобразовываем в управляемую строку
    var log := System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
    
    // И в конце обязательно освобождаем, чтобы не было утечек памяти
    System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
    gl.DeleteShader(Result);
    
    raise new System.ArgumentException($'{fname}:{#10}{log}');
  end;
  
end;

{$endregion Shader}

{$region Program}

function InitProgram(params shaders: array of gl_shader): gl_program;
begin
  Result := gl.CreateProgram;
  
  foreach var sh in shaders do
    gl.AttachShader(Result, sh);
  
  gl.LinkProgram(Result);
  // Всё то же самое что и у шейдеров
  var link_ok: integer;
  gl.GetProgramiv(Result, ProgramPropertyARB.LINK_STATUS, link_ok);
  if link_ok = 0 then
  begin
    
    var l: integer;
    gl.GetProgramiv(Result, ProgramPropertyARB.INFO_LOG_LENGTH, l);
    var ptr := System.Runtime.InteropServices.Marshal.AllocHGlobal(l);
    
    gl.GetProgramInfoLog(Result, l, IntPtr.Zero, ptr);
    var log := System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
    
    System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
    gl.DeleteProgram(Result);
    
    raise new System.ArgumentException(log);
  end;
  
end;

{$endregion Program}

end.