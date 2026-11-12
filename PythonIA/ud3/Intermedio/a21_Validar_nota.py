# Actividad 21

def validar_nota_examen(nota_texto):
    #Convierto el texto en float
    nota = float(nota_texto)
    # Si el usuario escribe letras, esto lanza ValueError automáticamente 
    if not (0 <= nota <= 10):
        raise ValueError(f"La nota {nota} no está en el rango válido (0-10).")
    return nota

print("=== GESTOR DE NOTAS ===")
while True:
    entrada = input("Introduce una nota (o escribe 'fin' para salir): ")

    #Uso el lower para que coge mayúsculas
    if entrada.lower() == "fin":
        print("Saliendo del programa...")
        break
        
    try:
        nota_valida = validar_nota_examen(entrada)
        print(f"Nota registrada correctamente: {nota_valida}")
    except ValueError as error:
        print(f"Error: {error}. Inténtalo de nuevo.")