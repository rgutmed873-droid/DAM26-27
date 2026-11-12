#Actividad 28

# Importamos la herramienta desde nuestro módulo
from validacion import validar_nota

print("=== REGISTRO DE NOTAS ===")


# Bucle infinito para pedir notas[cite: 1, 2]
while True:
    entrada = input("Introduce una nota (0-10): ")
    
    # Condición de salida[cite: 1, 2]
    if entrada.lower() == "fin":
        print("Cerrando el registro. ¡Hasta luego!")
        break # Rompe el bucle[cite: 1, 2]
        
    try:
        nota_num = float(entrada)
                
        validar_nota(nota_num)
                        
        # Si la función no lanzó ningún error, esta línea se ejecutará con normalidad
        print(f" Nota {nota_num} registrada correctamente.\n")
        print("Escribe 'fin' para salir.\n")
        
    except ValueError as error:
        # Este except captura DOS posibles fallos:
        # 1. Si el float() falla porque el usuario escribe letras (ej: "hola").
        # 2. Si la nota está fuera de rango y nuestra función dispara el 'raise ValueError'.
        print(f"Error detectado: {error}\n")