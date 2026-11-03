import sys

precio_texto = int(input("Precio del artículo: "))

descuento = 10 
precio_final = precio_texto - descuento 
print("El precio final es:", precio_final) 

#Da un error porque la variable de precio_texto lo está cogiendo como si fuera un tipo String
#Hay que cambiarlo a tipo entero para que pueda realizar el descuento y así la operación