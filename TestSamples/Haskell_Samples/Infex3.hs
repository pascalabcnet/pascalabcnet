module Main where

[] `plusplus` ys   = ys
x:xs `plusplus` ys = x:(xs`plusplus` ys)

main = do { 
              (print ([1,2] `plusplus` [3,4]));
          }