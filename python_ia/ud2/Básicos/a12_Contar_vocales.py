
palabra = input("Introduce una palabra: ")

vocales = ["a", "e", "i", "o", "u", "á", "é", "í", "ó", "ú"]
contador = 0

for letra in palabra.lower():
    if letra in vocales:
        contador += 1

print(f"La palabra {palabra} tiene {contador} vocales")