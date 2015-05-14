module Main where

g z = z+((\x -> s*x+y+((\z->z*z)5))3)+(let h=1;t=5 in h+t+y)
where y = 2;s = ((\x -> x*x+((\z->z*z)5))3)+((\x -> x*x+((\z->z*z)5))3)

main = do { 
              (print(g 1));
          }