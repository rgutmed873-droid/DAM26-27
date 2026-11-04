
producto = {}

producto["nombre"] = input("Nombre del producto: ")
producto["precio"] = float(input("Precio: "))
producto["stock"] = int(input("Stock: "))

print(f"Producto: {producto['nombre']} - Precio: {producto['precio']} € - Stock: {producto['stock']}")

producto["stock"] -= 1
print("Tras una venta:", producto)