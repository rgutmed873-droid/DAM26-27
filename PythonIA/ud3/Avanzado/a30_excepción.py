# Actividad 30: Excepción personalizada StockInsuficienteError

# 1. Definimos nuestra propia excepción
class StockInsuficienteError(Exception):
    pass

# 2. Función que procesa la venta y lanza los errores si algo va mal
def procesar_venta(producto, cantidad_a_vender, inventario_dict):
    """Comprueba si hay stock y simula la venta."""
    
    if producto not in inventario_dict:
        raise ValueError("El producto no existe en el catálogo.")
        
    stock_disponible = inventario_dict[producto]
    
    # Comprobamos si nos piden más de lo que tenemos
    if cantidad_a_vender > stock_disponible:
        # Lanzamos nuestro error personalizado
        raise StockInsuficienteError(f"Solo quedan {stock_disponible} unidades disponibles.")
        
    print(f"Venta confirmada: {cantidad_a_vender} unidad(es) de '{producto}'.")
    inventario_dict[producto] -= cantidad_a_vender


# 3. PROGRAMA PRINCIPAL
inventario = {}

# Cargamos los datos del archivo
try:
    with open("inventario_a30.txt", "r", encoding="utf-8") as archivo:
        for linea in archivo:
            prod, cant = linea.strip().split(",")
            inventario[prod] = int(cant)
except FileNotFoundError:
    print("Error: No se ha encontrado el archivo inventario_a30.txt")

print("=== SISTEMA DE VENTAS ===")

while True:
    print(f"\nStock actual: {inventario}")
    producto_pedido = input("Producto a vender (o 'fin'): ").lower()
    
    if producto_pedido == "fin":
        print("Cerrando caja.")
        break
        
    try:
        cantidad_pedida = int(input("¿Cuántas unidades?: "))
        
        # Llamamos a la función
        procesar_venta(producto_pedido, cantidad_pedida, inventario)
        
    # Capturamos primero nuestro error personalizado de stock
    except StockInsuficienteError as error:
        print(f"ERROR DE STOCK: {error}")
        
    # Capturamos después los errores genéricos (producto no existe o letras en cantidad)
    except ValueError as error:
        print(f"ERROR: {error}")