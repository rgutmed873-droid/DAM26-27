
def anadir_tarea(tareas):
    texto = input("Texto de la tarea: ")
    tareas.append({"texto": texto, "hecha": False})
    print("Tarea añadida")


def mostrar_tareas(tareas):
    if len(tareas) == 0:
        print("No hay tareas")
    else:
        for numero, tarea in enumerate(tareas, 1):
            if tarea["hecha"]:
                estado = "Hecha"
            else:
                estado = "Pendiente"
            print(f"{numero}. {tarea['texto']} - {estado}")


def completar_tarea(tareas):
    if len(tareas) == 0:
        print("No hay tareas para completar")
    else:
        mostrar_tareas(tareas)
        numero = int(input("Número de la tarea a completar: "))
        if numero >= 1 and numero <= len(tareas):
            tareas[numero - 1]["hecha"] = True
            print("Tarea completada")
        else:
            print("Número no válido")


tareas = []
opcion = ""

while opcion != "4":
    print()
    print("1. Añadir tarea")
    print("2. Mostrar tareas")
    print("3. Completar tarea")
    print("4. Salir")
    opcion = input("Elige una opción: ")

    if opcion == "1":
        anadir_tarea(tareas)
    elif opcion == "2":
        mostrar_tareas(tareas)
    elif opcion == "3":
        completar_tarea(tareas)
    elif opcion == "4":
        print("Hasta luego")
    else:
        print("Opción no válida")