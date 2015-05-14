module Main where

sign z | z==0  = 0
       | z>0  = 1
       | z<0  = -1

main = do { 
              (print(sign 5));
              (print(sign 0));
              (print(sign -10));             
          }