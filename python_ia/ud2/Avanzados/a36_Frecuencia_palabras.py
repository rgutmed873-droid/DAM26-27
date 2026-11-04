
texto = ("el perro ladra, el gato maulla y la vaca muge")
palabras = texto.split()
frecuencia = {}

for palabra in palabras:
    if palabra in frecuencia:
        frecuencia[palabra] += 1
    else:
        frecuencia[palabra] = 1

for palabra, veces in frecuencia.items():
    print(palabra, "->", veces)