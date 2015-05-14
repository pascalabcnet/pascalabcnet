module Main where

g z = (let x=2 in x*x+z+(q x))
--where q x | x<=2 = 4*x+s | x>2 = 7 where s=8
where q z = 4*z+((\x y -> x+y+z)3 4)

main = do { 
              (print(g 1));
              (print(g 3));
          }