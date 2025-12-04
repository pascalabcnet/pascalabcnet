type
  t1 = abstract class(System.Collections.IEnumerator) 
    // Падает только если НЕ указать System.Collections
    public property System.Collections<integer>.IEnumerator.Current: object read nil;
    
  end;
  
begin end.