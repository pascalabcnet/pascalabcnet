module Main where

g 1 = let x=2 in x*x+q
where q = 4+(let x=2 in x*x)+(s 1)+t 
      where t=4;
            s x =4+j+(let x=2 in x*x) 
            where i=0;j=5*k+qq
                      where k=0;
                            qq = 4+(let x=2 in x*s) 
                            where s=4
g z = let x=2 in x*x+q+z
where q = 4+(let x=2 in x*x)+(s 3)+t 
      where t=4;
            s x =4+j+(let x=2 in x*x)+x
            where i=0;j=5*k+(qq 2)
                      where k=0;
                            qq y = 4+(let x=2 in x*s)+y 
                            where s=4
                            
main = do { 
              (print(g 1));
              (print(g 3));
          }