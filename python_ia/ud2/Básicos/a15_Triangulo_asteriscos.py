
filas = int(input("Número de filas: "))

for i in range(1, filas + 1):
    linea = ""
    for j in range(i):
        linea += "*"
    print(linea)