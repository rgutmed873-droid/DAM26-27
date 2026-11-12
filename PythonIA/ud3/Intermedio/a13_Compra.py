#Actividad 13

#Inicio la variable del total de la compra ha 0 porque no hay nada
total_compra = 0.0

#Abro el archivo
with open("compra.txt", "r", encoding="utf-8") as archivo:

    #Recorro con un bucle for las lineas de la lista
    for linea in archivo:
        
        producto, precio = linea.strip().split(",")
        
        total_compra += float(precio)

#Muestro el resultado final
print(f"El importe total de la compra es: {total_compra:.2f} €")        

