
compra = []

while True:
    producto = input("Introduce un producto: (escribe fin para terminar): ")
    if producto == "fin":
        break
    compra.append(producto)

for numero, item in enumerate(compra,1):
        print(f"{numero}. {item}")

print(f"Total de productos: {len(compra)}")