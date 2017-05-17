{$reference System.Speech.dll}
unit Speech;

interface

procedure Speak(s: string);

procedure SpeakAsync(s: string);

implementation

uses System.Speech.Synthesis;

var ss: SpeechSynthesizer;

procedure Speak(s: string);
begin
  ss.Speak(s)
end;

procedure SpeakAsync(s: string);
begin
  ss.SpeakAsync(s)
end;

begin
  ss := new SpeechSynthesizer;
  var voices := ss.GetInstalledVoices;
  ss.SelectVoice(voices[0].VoiceInfo.Name)
end.