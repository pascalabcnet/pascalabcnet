module Main where

sign 0  2:3 = 0
sign z | 2  = 1
sign z | z < 0  = -1

main = do { 
              (print (sign 5));
              (print (sign 0));
              (print (sign -10));
          }

