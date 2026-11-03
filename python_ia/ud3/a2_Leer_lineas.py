
#Actividad 2

# 1. Abrimos el archivo que acabas de crear en modo lectura ("r")
with open("poema.txt", "r", encoding="utf-8") as archivo:
    
    # 2. Recorremos el archivo línea a línea, numerándolas desde el 1
    for numero, linea in enumerate(archivo, start=1):
        
        # 3. Limpiamos la línea para quitarle el intro invisible del final
        linea_limpia = linea.strip()
        
        # 4. TAREA: Haz un print usando un f-string para mostrar el resultado.
