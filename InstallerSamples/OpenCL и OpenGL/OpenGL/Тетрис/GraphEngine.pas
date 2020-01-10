unit GraphEngine;

interface

uses OpenGL;

type
  ColorIndex = integer;
  
  gr = static class
    
    private static ColorTable: array of Vec3ub;
    public static Field: array[,] of ColorIndex;
    
    public static procedure MiscInit;
    
    public static procedure Resize;
    
  end;

implementation

{$reference System.Windows.Forms.dll}
{$reference System.Drawing.dll}
uses System.Windows.Forms;
uses System.Drawing;

uses System;

var gl: OpenGL.gl;

{$region Buffer}

function InitBuffer(bt: BufferBindType; sz: integer; data: &Array): BufferName;
begin
  gl.CreateBuffers(1, Result);
  
  gl.NamedBufferData(Result, new UIntPtr(sz), data, BufferDataUsage.STATIC_DRAW);
  
end;

{$endregion Buffer}

{$region Texture}

function CreateCellTexture(w: integer): TextureName;
begin
  gl.GenTextures(1,Result);
  gl.BindTexture(TextureBindTarget.T_2D, Result);
  
  gl.TexImage2D(TextureBindTarget.T_2D, 0, InternalDataFormat.RGB8, w,w, 0, DataFormat.RGBA, DataType.UNSIGNED_BYTE, MatrGen(w,w,(x,y)->
  begin
    Result := new Vec4ub(0,0,0,1); //ToDo попробовать использовать Vec3, всё равно нужны только R,G и B
  end));
  
  // в данном примере текстура не маштабируется, поэтому можно взять более простой фильтр
  gl.TextureParameteri(Result, TextureInfoType.MIN_FILTER, PixelFilterMode.NEAREST);
  gl.TextureParameteri(Result, TextureInfoType.MAG_FILTER, PixelFilterMode.NEAREST);
  
  // а тут вообще не важно какие значения, текстурные координаты у нас не выходят за границы текстуры
  // но, если я правильно понимаю - для REPEAT не нужны даже сами проверки выхода за границы
  // поэтому, мелочь, но всё же разгрузка GPU от ненужного
  gl.TextureParameteri(Result, TextureInfoType.WRAP_S, PixelWrapMode.REPEAT);
  gl.TextureParameteri(Result, TextureInfoType.WRAP_T, PixelWrapMode.REPEAT);
  
end;

{$endregion Texture}

{$region Shader}

function InitShader(fname: string; st: ShaderType): ShaderName;
begin
  Result := gl.CreateShader(st);
  
  var source := ReadAllText(fname);
  
  // в данной версии модуля OpenGL параметры принимающие массив строк - не поддерживаются
  // поэтому надо ручками преобразовывать управляемую строку в неуправляемую с кодировкой ANSI 
  var source_strptr := System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(source);
  var source_len := source.Length; // нужна отдельная переменная чтобы у значения был адрес на стеке, gl.ShaderSource принимает именно адрес
  
  gl.ShaderSource(Result, 1, source_strptr, source_len);
  
  // обязательно освобождаем память, иначе утечка памяти
  System.Runtime.InteropServices.Marshal.FreeHGlobal(source_strptr);
  
  gl.CompileShader(Result);
  // получаем состояние успешности компиляции
  // 1=успешно
  // 0=ошибка
  var comp_ok: integer;
  gl.GetShaderiv(Result, ShaderInfoType.COMPILE_STATUS, comp_ok);
  if comp_ok = 0 then
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
    Writeln($'Ошибка создания шейдера {st}');
    Writeln(log);
    
    // и опять же, в конце обязательно освобождаем память, чтоб не было утечек памяти
    System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
  end;
  
end;

{$endregion Shader}

{$region Program}

function InitProgram(vertex_shader, fragment_shader: ShaderName): ProgramName;
begin
  Result := gl.CreateProgram;
  
  gl.AttachShader(Result, vertex_shader);
  gl.AttachShader(Result, fragment_shader);
  
  gl.LinkProgram(Result);
  // всё то же самое что и у шейдеров
  var link_ok: integer;
  gl.GetProgramiv(Result, ProgramInfoType.LINK_STATUS, link_ok);
  if link_ok = 0 then
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

{$region gr implementation}

static procedure gr.MiscInit;
begin
  
  // 7 видов фигур, у каждой фигуры свой цвет
  ColorTable := new Vec3ub[7](
    new Vec3ub($00, $F0, $F0),  // I
    new Vec3ub($00, $00, $F0),  // J
    new Vec3ub($F0, $A0, $00),  // L
    new Vec3ub($F0, $F0, $00),  // O
    new Vec3ub($00, $F0, $00),  // S
    new Vec3ub($A0, $00, $F0),  // T
    new Vec3ub($F0, $00, $00)   // Z
  );
  
end;

static procedure gr.Resize;
begin
  
end;

{$endregion gr implementation}

end.