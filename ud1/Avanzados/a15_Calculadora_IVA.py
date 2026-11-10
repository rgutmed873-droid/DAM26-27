import sys

precio_Sin_IVA = float(input("Dime el precio del producto: "))

iva = precio_Sin_IVA * 0.21

precio_IVA = precio_Sin_IVA + iva

propina = float(input("Dime el porcentaje de propina: "))

calculo_propina = (precio_IVA * propina/100 +1)

precio_final = precio_IVA + calculo_propina

print(f"El precio sin IVA es: {precio_Sin_IVA}")
print(f"El precio con IVA y sin propina es: {precio_IVA}")
print(f"El precio con IVA y la propina es: {precio_final}")

