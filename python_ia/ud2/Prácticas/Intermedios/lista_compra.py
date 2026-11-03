
compra = []

while True:
    producto = input("Introduce un producto (o escribe fin para terminar): ")

    if producto == "fin":
        break
    else:
        compra.append(producto)

print("\n Tu lista de la compra es: ")
for i, item in enumerate(compra, 1):
    print(f"{i} - {item}")

print(f"\nTienes un total de {len(compra)} productos en tu lista")    
