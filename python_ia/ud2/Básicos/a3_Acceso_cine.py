
edad = int(input("Introduce tu edad: "))

estudiante = input("¿Eres estudiante (si/no)?: ").lower

if edad < 18 or estudiante == "si":
    print("Tienes descuento")
else:
    print("No tienes el descuento")    

#Explicación con if anidados

#if edad <18
#print("Tienes descuento")
#else:
#if estudiante == "si":
#print("Tienes descuento")
#else
#print("No tienes descuento")