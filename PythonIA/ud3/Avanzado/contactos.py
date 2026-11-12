# contactos.py

class ContactoNoEncontradoError(Exception):
    """Excepción lanzada cuando no se encuentra un contacto en la agenda."""
    pass

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
            raise ContactoNoEncontradoError(f"No existe ningún contacto llamado '{nombre_buscado}'")
            
    except FileNotFoundError:
        print("La agenda está vacía. Aún no hay contactos guardados.")
    except ValueError:
         print("Error: El archivo de contactos tiene líneas mal formateadas.")

def eliminar_contacto(nombre_borrar):
    """Busca un contacto y lo elimina del archivo. Lanza error si no existe."""
    try:
        with open("contactos.txt", "r", encoding="utf-8") as archivo:
            lineas = archivo.readlines()
    except FileNotFoundError:
        raise ContactoNoEncontradoError("La agenda está vacía, no hay contactos para eliminar.")

    contacto_encontrado = False
    lineas_a_guardar = []

    for linea in lineas:
        if not linea.strip():
            continue
            
        nombre, telefono = linea.strip().split(",")
        # Comparamos ignorando mayúsculas/minúsculas
        if nombre.lower() == nombre_borrar.lower():
            contacto_encontrado = True
        else:
            lineas_a_guardar.append(linea)

    if not contacto_encontrado:
        # Si terminamos de leer y no estaba, lanzamos nuestra excepción
        raise ContactoNoEncontradoError(f"No se puede eliminar: el contacto '{nombre_borrar}' no existe")

    # Si lo hemos encontrado, sobrescribimos el archivo con la lista limpia
    with open("contactos.txt", "w", encoding="utf-8") as archivo:
        archivo.writelines(lineas_a_guardar)
    
    print(f"Contacto '{nombre_borrar}' eliminado correctamente del archivo.")         