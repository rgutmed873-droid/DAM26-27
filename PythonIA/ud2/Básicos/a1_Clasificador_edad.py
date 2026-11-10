import sys

edad = int(input("Introduce tu edad: "))

if edad <=2: print("Bebé")
elif edad <=12: print("Niño")
elif edad <= 17: print("Adolescente")
elif edad <= 64: print("Adulto")
else: print("Persona Mayor")
