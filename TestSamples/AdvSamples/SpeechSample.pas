//.NET 3.0 requires
#reference 'System.Speech.dll'

uses System.Speech, System.Speech.Synthesis;

begin
  var sp : SpeechSynthesizer := new SpeechSynthesizer();
  sp.Speak('Hello from .NET');
end.