module Main where

sign 0   = 0
sign z | z > 0  = 1
sign z | z > 0  = -1

main = do { 
              (print(sign 5));
              (print(sign 0));
              (print(sign -10));
          }

