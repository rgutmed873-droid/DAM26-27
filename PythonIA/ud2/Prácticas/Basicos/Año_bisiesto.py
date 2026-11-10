
"""
Pide un año al usuario y determina si es bisiesto usando una condición compuesta. 
Recuerda la regla matemática: un año es bisiesto si es divisible entre 4, salvo que sea divisible entre 100 y no entre 400.
"""

anio = int(input("Introduce un año: "))

if (anio % 4 == 0 and anio % 100 != 0) or (anio % 400 == 0):
    print(f"El {anio} es un año bisiesto")
else:
    print(f"El {anio} no es un año bisiesto")    