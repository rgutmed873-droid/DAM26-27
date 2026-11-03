#Opcion 1
#Rafael Gutiérrez -[Y] = 7, [Z] = 3
#Funcion en el que calculo el stock total que tiene la tienda
"""
Lo primero de todo defino la lista total para almacenar todos los datos que tiene el diccionario de almacén
Después hago un bucle for que recorra cada pasillo dentro de almacén
Lo siguiente es que en total vaya sumandose cada pasillo y de cada pasillo obtenta los productos
Por último devuelve un return total para que se muestre el total
"""
def stock_total(almacen, producto):
    total = 0
    for pasillo in almacen:
        total += almacen[pasillo].get(producto,0)
    return total


#Funcion para determinar que productos está critico
"""
Creo la lista de criticos
Luego he hecho un bucle for en el que recorro los pasillo de almacen y dentro de ese bucle otro bucle de los productos dentro del almacen que
mire los pasillos
Con un if si el producto esta dentro de critico que en la función he definido que el minimo de productos es 20 con un continue salta de sección
Y si el stock_total que hay en el almacen de un producto es menor que el minimo se añade a la lista de criticos productos.
Y devuelve esa lista
"""
def producto_criticos(almacen, minimo=20):
    criticos = []
    for pasillo in almacen:
        for producto in almacen[pasillo]:
            if producto in criticos:
                continue
            if stock_total(almacen, producto) < minimo:
                criticos.append(producto)
        return criticos


#Funcion para reponer productos en el almacen
"""
Lo primero que he hecho ha sido crear la variable de lotes que la he igualado a 0 porque es por donde vamos a empezar
Lo siguiente he creado un bucle while en el que el diccionario de almacen obteniendo un pasillo y de ese pasillo su producto es menor que el
minimo de stock
Primero compruebo con un if si los lotes de los productos son igual al max hago un break para que termine
Si no es así si en el pasillo el producto que hay es igual que al producto que ha seleccionado más el lote se añade uno al lote para tener stock.
"""
def reponer(almacen, pasillo, producto, minimo, lote=12, max= 5):
    lotes = 0
    while almacen[pasillo].get(producto,0) < minimo:
        if lotes == max:
            break
        almacen[pasillo][producto] = almacen[pasillo].get(producto, 0) + lote
        lotes +=1
    return lotes
    


#Diccionario en el que almaceno los datos
almacen = {
    "A1": {"tornillos": 15, "tuercas": 8, "clavos": 40},
    "A2": {"tornillos": 10, "martillos": 6, "cinta": 25},
    "B1": {"tuercas": 5, "pintura": 30, "brochas": 12},
    "B2": {"pintura": 18, "rodillos": 9},
    "C1": {"cinta": 14, "guantes": 22, "brochas": 3},
    "C2": {"clavos": 12, "martillos": 4, "guantes": 7},
}

#Muestra los productos que ya existen en el pasillo
print("Stock total de tornillos (repetido):", stock_total(almacen, "tornillos"))
print("Stock total de taladros (inexistente):", stock_total(almacen, "taladros"))

 
print("Críticos (mínimo por defecto = 20):", producto_criticos(almacen))
print("Críticos (mínimo = 50):", producto_criticos(almacen, minimo=50))
 

print("\nStock de tuercas en A1 antes:", almacen["A1"]["tuercas"])
lotes = reponer(almacen, "A1", "tuercas", 30)
print("Lotes añadidos:", lotes)
print("Stock de tuercas en A1 después:", almacen["A1"]["tuercas"])
print("Stock total de tuercas ahora:", stock_total(almacen, "tuercas"))

print("\n¿Hay tornillos en B2 antes?", "tornillos" in almacen["B2"])
lotes = reponer(almacen, "B2", "tornillos", 30)
print("Lotes añadidos:", lotes)
print("Stock de tornillos en B2 después:", almacen["B2"]["tornillos"])
print("Stock total de tornillos ahora:", stock_total(almacen, "tornillos"))
 

lotes = reponer(almacen, "C2", "martillos", 100)
print("\nExtra, mínimo inalcanzable -> lotes:", lotes,
      "| martillos en C2:", almacen["C2"]["martillos"])


