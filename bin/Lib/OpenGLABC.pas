///
///Модуль, зарезервированный для высокоуровневой оболочки модуля OpenGL
///
unit OpenGLABC;

interface

uses OpenGL;
uses OpenGLABCBase in 'Internal\OpenGLABCBase';

implementation

begin
  Writeln('OpenGLABC');
  Writeln(new System.NotImplementedException);
  Readln;
  Halt;
end.