#La Mini Agenda Telefónica
"""
Crea un diccionario vacío llamado agenda.

Inicia un bucle infinito (while True) para mantener el programa funcionando.

Dentro del bucle, muéstrale al usuario un menú con 3 opciones (puedes usar varios print):

1. Añadir un contacto

2. Ver todos los contactos

3. Salir

Pide al usuario que elija una opción usando un input().

Crea una cadena de if/elif/else para manejar las opciones:

Si elige "1": Pide al usuario un nombre (será la clave) y un teléfono (será el valor), y guárdalos en el diccionario agenda.

Si elige "2": Recorre el diccionario con un bucle for y .items(), mostrando por pantalla Nombre: [nombre] - Teléfono: [teléfono].

Si elige "3": Muestra un mensaje de despedida y rompe el bucle con break.

Si escribe otra cosa: Muestra un mensaje de "Opción no válida".
"""
agenda = {}

while True:
    print("\n MENU DE LA AGENDA")
    print("1. Añadir un contacto")
    print("2. Ver todos los contactos")
    print("3. Salir del programa")

    opcion = input("Elige una opción (entre 1,2,3): ")

    if opcion == "1":

        nombre = input("Introduce tu nombre: ")
        telefono = input("Introduce tu teléfono: ")
        agenda[nombre] = telefono
        print("Contacto guardado")

    elif opcion == "2":
        print("\n LISTA DE CONTACTOS ")
        for nombre, telefono in agenda.items():
            print(f"Nombre: {nombre} | Teléfono: {telefono}")

    elif opcion == "3":
        print("Saliendo del programa")
        break

    else:
        print("Opción no valida")


