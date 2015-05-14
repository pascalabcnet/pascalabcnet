module Main where
import PreludeModule 
import PreludeModule1

main = do { 
              (print(max 5 8));
              (print(([1,2,3] `plusplus` [4,5,6,7])));
          }