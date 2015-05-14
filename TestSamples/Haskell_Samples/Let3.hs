module Main where

f = let x=5; y=2-x; g x | x>2 = x*x | x<=2 = x*4 in x+(g y)

h = (let g x | x<=2 = 6+s | x>2 = 7 | x>3 = 8 where s=7 in (g 2)+(g 3)+(g 4)) +
    (let g x | x<=2 = 6 | x>2 = 7 | x>3 = 8 in (g 2)+(g 3)+(g 4))

main = do {
            (print f);
            (print h);
          }