#Opcion 2
#Rafael Gutiérrez -[Y] = 7, [Z] = 3

def siguiente(cola):
    
    mejor = 0
    for i in range(1, len(cola)):
       
        if cola[i][1] < cola[mejor][1]:
            mejor = i
    return mejor


def reclasificar(cola, *nombres):
    
    reclasificados = 0
    for nombre in nombres:
        for i in range(len(cola)):
            if cola[i][0] == nombre:
                vieja = cola[i]
                nueva = (vieja[0], 1, vieja[2])
                cola.insert(i, nueva)   # la nueva ocupa la misma posición
                cola.remove(vieja)      # se elimina la vieja (queda en i + 1)
                reclasificados += 1
                break
    return reclasificados


def atender(cola, jornada=240):
    
    atendidos = []
    total = 0
    while len(cola) > 0:
        paciente = cola[siguiente(cola)]
        nombre, prioridad, minutos = paciente

        if minutos == 0:                 # no se ha presentado
            cola.remove(paciente)
            continue

        if total + minutos > jornada:    # no cabe en la jornada
            break

        cola.remove(paciente)
        total += minutos
        atendidos.append((nombre, total))
    return atendidos


# ---------------- Programa principal ----------------
cola = [("Lucía", 3, 15), ("Mario", 2, 20), ("Irene", 1, 30), ("Pablo", 3, 0),
        ("Sara", 2, 25), ("Hugo", 1, 10), ("Nora", 3, 20)]

Z = 3

num_reclasificados = reclasificar(cola, "Nora", "Pablo", "Ana")
atendidos = atender(cola, jornada=60 + 5 * Z)

print("Reclasificados:", num_reclasificados)
print("Atendidos:", atendidos)
print("Sin atender:", [paciente[0] for paciente in cola])
