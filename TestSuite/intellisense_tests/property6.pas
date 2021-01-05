uses System.Windows.Media.Animation;

{$reference PresentationFramework.dll}
{$reference PresentationCore.dll}
{$reference WindowsBase.dll}

{$apptype windows}

begin
  var ac: AnimationClock;
  ac.CurrentTime{@property Clock.CurrentTime: System.Nullable<TimeSpan>; readonly;@};
end.