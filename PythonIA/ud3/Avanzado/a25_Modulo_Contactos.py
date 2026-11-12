#Actividad 25


#Aquí es el main del programa y donde va a ejecutarse todo

# Importamos las funciones exactamente desde 'contactos'
from contactos import añadir_contacto, listar_contactos, buscar_contacto

while True: # Iniciamos el bucle del menú[cite: 1, 2]
    print("\n=== AGENDA DE CONTACTOS ===")
    print("1. Añadir contacto")
    print("2. Listar contactos")
    print("3. Buscar contacto")
    print("4. Salir")
    
    opcion = input("Elige una opción (1-4): ")
    
    if opcion == "1":
        nombre = input("Introduce el nombre: ")
        telefono = input("Introduce el teléfono: ")
        añadir_contacto(nombre, telefono)
        
    elif opcion == "2":
        listar_contactos()
        
    elif opcion == "3":
        nombre = input("Introduce el nombre a buscar: ")
        buscar_contacto(nombre)
        
    elif opcion == "4":
        print("¡Saliendo de la agenda! Hasta pronto.")
        break # Terminamos el programa[cite: 2]
        
    else:
        print("Opción no válida. Inténtalo de nuevo.")