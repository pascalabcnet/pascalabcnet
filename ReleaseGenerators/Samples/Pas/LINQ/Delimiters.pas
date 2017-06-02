begin
  Range(#0,#127).Where(c->char.IsLetterOrDigit(c)).Println;
  Range(#0,#127).Where(c->char.IsPunctuation(c)).Println;
end.