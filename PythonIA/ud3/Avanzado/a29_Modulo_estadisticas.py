#Actividad 29

# main.py

#importar las funciones del otro archivo
from estadisticas import media, mayor, menor

lista_numeros = []

try:
    with open("datos.txt", "r", encoding="utf-8") as archivo:
        for linea in archivo:
            numero = float(linea.strip())
            lista_numeros.append(numero)
            
            
except FileNotFoundError:
    print("Error: No se ha encontrado el archivo 'datos.txt'.")
except ValueError:
    print("Error: El archivo contiene datos que no son números")    

# Si hemos conseguido leer números, mostramos las estadísticas
if len(lista_numeros) > 0:
    print(f"=== ESTADÍSTICAS ===")
    print(f"Números leídos: {lista_numeros}\n")
    
    print(f"Media: {media(lista_numeros):.2f} ")
    print(f"Número mayor: {mayor(lista_numeros):.2f} ")
    print(f"Número menor: {menor(lista_numeros):.2f} ")
    
else:
    print("No hay datos para calcular.")