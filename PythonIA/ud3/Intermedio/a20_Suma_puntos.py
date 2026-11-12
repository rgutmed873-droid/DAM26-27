#Actividad 20

total_puntos = 0

with open("puntos.txt", "r", encoding="utf-8") as archivo:
    for linea in archivo:
        try:
            puntos = int(linea.strip())
            total_puntos += puntos
        except ValueError:
            #Validación del tipo de dato
            print(f"La linea leída es un texto")

print(f"\n La puntuación total válida es: {total_puntos} puntos.")                
