unit Sounds;

{$reference 'PresentationCore.dll'}

procedure PlaySound(fname: string);
begin
  var fsound := new System.Windows.Media.MediaPlayer;
  fsound.Open(new System.Uri(fname,System.UriKind.RelativeOrAbsolute));
  fsound.Play;
end;

type Sound = class
  fsound := new System.Windows.Media.MediaPlayer;
public
  constructor (fname: string);
  begin
    Open(fname);
  end;
  procedure Open(fname: string) := fsound.Open(new System.Uri(fname,System.UriKind.RelativeOrAbsolute));
  procedure Play := fsound.Play;
  procedure Stop := fsound.Stop;
  procedure Pause := fsound.Pause;
  procedure Reset := fsound.Position := new System.Timespan(0);
end;

end.