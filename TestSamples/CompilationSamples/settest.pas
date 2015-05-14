var s: set of string;

begin
  s := ['бегемот','жираф','крокодил'];
  s := s + ['жираф','крокодил','какаду'];
  if 'какаду' in s then 
    write('какаду');
  write(s);
end.
