
"""""
Pide al usuario su edad por teclado y muestra por pantalla 
"Bebé" (0-2 años), "Niño/a" (3-12 años), "Adolescente" (13-17 años), "Adulto" (18-64 años) o "Persona mayor" (65 o más años), 
utilizando una única cadena de if/elif/else.
"""

edad = int(input("Introduce tu edad: "))

if edad <= 2:
    print("Bebé")
elif edad <=12:
    print("Niño")
elif edad <=17:
    print("Adolescente")
elif edad <=64:
    print("Adulto")
else:
    print("Persona mayor")            