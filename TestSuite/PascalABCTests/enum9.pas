type
  Days = (
    None      = 0,  // 0
    Monday    = 1,  // 1
    Tuesday   = 2,  // 2
    Wednesday = 4,  // 4
    Thursday  = 8,  // 8
    Friday    = 16,  // 16
    Saturday  = 32,  // 32
    Sunday    = 64,  // 64
    Weekend   = Saturday or Sunday
  );

begin 
 var d : Days := Saturday;
 var meetingDays := Days.Monday or Days.Friday;
 assert((d and Days.Weekend) = Saturday);
 assert((meetingDays and Days.Friday) = Days.Friday);
end.