--module Main where

g x y | x>5       = x+y
      | x<0 = -1
g x y | x<5       = -3
      | x=5 = -2
where z=1

f 1 = 0
f a = a+z
where z=8

main = do { 
              (print(g 6 2));
              (print(g 0 2));
              (print(f 1));
              (print(f 4));
          }