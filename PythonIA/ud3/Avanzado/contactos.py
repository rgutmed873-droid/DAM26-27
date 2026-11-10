# contactos.py

def añadir_contacto(nombre, telefono):
    """Función para añadir un contacto al documento, protegida contra fallos."""
    try:
        with open("contactos.txt", "a", encoding="utf-8") as archivo:
            # Escribo el nombre y el teléfono separado por comas
            # \n para que sea cada uno en una linea
            archivo.write(f"{nombre},{telefono}\n")
        print(f"Contacto '{nombre}' guardado correctamente.")
    except Exception as error:
        print(f"Error inesperado al guardar el archivo: {error}")

def listar_contactos():
    """Lee y muestra todos los contactos guardados, a prueba de errores."""
    try:
        print("\n--- Lista de Contactos ---")
        with open("contactos.txt", "r", encoding="utf-8") as archivo:
            for linea in archivo:
                nombre, telefono = linea.strip().split(",")
                print(f"Nombre: {nombre} | Teléfono: {telefono}")
    except FileNotFoundError:
        print("La agenda está vacía. Aún no hay contactos guardados.")
    except ValueError:
        print("Error: El archivo de contactos tiene líneas mal formateadas (falta alguna coma).")

def buscar_contacto(nombre_buscado):
    """Busca un contacto por nombre de forma segura."""
    encontrado = False
    try:
        with open("contactos.txt", "r", encoding="utf-8") as archivo:
            for linea in archivo:
                nombre, telefono = linea.strip().split(",")
                if nombre.lower() == nombre_buscado.lower():
                    print(f"Encontrado: {nombre} -> {telefono}")
                    encontrado = True
                    break
                    
        if not encontrado:
            print(f"El contacto '{nombre_buscado}' no existe.")
            
    except FileNotFoundError:
        print("La agenda está vacía. Aún no hay contactos guardados.")
    except ValueError:
         print("Error: El archivo de contactos tiene líneas mal formateadas.")