
import random

secreto = random.randint(1, 100)
intentos = 0
acertado = False

while not acertado and intentos < 10:
    intento = int(input("Adivina el número (1-100): "))
    intentos += 1

    if intento == secreto:
        acertado = True
    elif intento < secreto:
        print("Mayor")
    else:
        print("Menor")

if acertado:
    print(f"¡Correcto! Lo has adivinado en {intentos} intentos")
else:
    print(f"Te has quedado sin intentos. El número era {secreto}")