
def resumen(*numeros):
    suma = sum(numeros)
    if len(numeros) == 0:
        media = 0
    else:
        media = suma / len(numeros)
    return (suma, media)

resultado = resumen(4, 6)
print(f"Suma: {resultado[0]}, media: {resultado[1]}")

resultado = resumen(5, 7, 8, 3, 9)
print(f"Suma: {resultado[0]}, media: {resultado[1]}")

resultado = resumen()
print(f"Suma: {resultado[0]}, media: {resultado[1]}")        