module PreduleModule where

eq x y = x==y

noteq x y = not(x==y)

max x y  | x <= y    =  y
max x y  | x > y     =  x
         
min x y  | x <= y    =  x
min x y  | x > y     =  y

succ x = x+1

pred x = x-1

condition x = x>4

until p f x | p(x)       =  x
until p f x | otherwise  =  until(p f f(x))

map f []     = []
map f x:xs = f(x) : map(f xs)

filter p []                       = []
filter p x:xs | p(x)              = x : filter(p xs)
filter p x:xs | otherwise  = filter(p xs)

head []         =  "Prelude.head: пустой список"
head x:xs       =  x

tail []         =  "Prelude.tail: пустой список"
tail x:xs       =  xs

length [] = 0
length h:t = length (t) + 1

init []           =  "Prelude.init: пустой список"
init x | length(x)==1       =  []
init x | otherwise       =  head(x) : init(tail(x))

f x y = x+y

foldr a x [] = x
foldr a x h:t = a(h foldr(a x t))

foldl a z []     =  z
foldl a z x:xs =  foldl(a a(z x) xs)

