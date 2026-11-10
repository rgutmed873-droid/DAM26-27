#Actividad 27

# Función de ayuda para registrar el error en el archivo
def registrar_error(mensaje):
    """Guarda el mensaje de error en errores.log con un salto de línea."""
    with open("errores.log", "a", encoding="utf-8") as archivo:
        archivo.write(f"{mensaje}\n")
    
    pass

print("=== CALCULADORA: 100 / x ===")
print("Escribe 'fin' para terminar el programa.\n")

# Bucle infinito para pedir números hasta que el usuario escriba 'fin'[cite: 1, 2]
while True:
    entrada = input("Introduce un número para dividir a 100: ")
    
    if entrada.lower() == "fin":
        print("Finalizando programa. Revisa el archivo errores.log si hubo problemas.")
        break # Salimos del bucle[cite: 1, 2]
        
    try:
        # Intentamos convertir la entrada y hacer la división
        numero = float(entrada)
        resultado = 100 / numero
        print(f"El resultado es: {resultado:.2f}\n")
        print("Escribe fin para terminar el programa")
        
    except ValueError:
        registrar_error(f"Error: se intentó divir entre texto en vez de núemro")
        print(" Ha ocurrido un error (texto no numérico). Se ha guardado en el log.\n")
        pass
        
    except ZeroDivisionError:
        registrar_error(f"Error no se puede dividir entre 0")
        print(" Ha ocurrido un error (división por cero). Se ha guardado en el log.\n")
        pass