
año = int(input("Dime un año: "))

if año % 4 == 0:
    if año % 100 == 0 and año %400 != 0 : print("Año bisiesto")
    else: print("No es un año bisiesto")    
