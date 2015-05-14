module Main where

g z = z+(let h=1 in h+5)+((\x -> x*x+s)3)+(let h=1 in h+5)
where s = (let h=1 in h+5)+((\x -> x*x+((\z->z*z)5))3)

main = do { 
              (print(g 1));
          }