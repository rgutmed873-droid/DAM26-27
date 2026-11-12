# Actividad 32

# Importamos tus módulos reales usando sus nombres exactos
import a31_Modulo_Fechas as fechas
import contactos
from contactos import ContactoNoEncontradoError

while True:
    print("\n--- MENÚ PRINCIPAL ---")
    print("1. Buscar teléfono")
    print("2. Eliminar contacto") 
    print("3. Validar un cumpleaños")
    print("4. Salir")
    opcion = input("Elige opción: ")
    
    if opcion == "1":
        nombre = input("Dime el nombre a buscar: ")
        try:
            telefono = contactos.buscar_contacto(nombre)
            print(f"El teléfono de {nombre} es {telefono}")
        except ContactoNoEncontradoError as error:
            print(f"{error}")
            
    elif opcion == "2":
        nombre = input("Dime el nombre a eliminar: ")
        try:
            contactos.eliminar_contacto(nombre)
        except ContactoNoEncontradoError as error:
            print(f"{error}")
            
    elif opcion == "3":
        fecha = input("Dime una fecha de cumple (DD/MM/AAAA): ")
        try:
            fechas.validar_fecha_dd_mm_aaaa(fecha)
            print("¡Fecha registrada con éxito!")
        except ValueError as error:
            print(f"Error de fecha: {error}")
            
    elif opcion == "4":
        print("Saliendo...")
        break
    else:
        print("Opción incorrecta.")