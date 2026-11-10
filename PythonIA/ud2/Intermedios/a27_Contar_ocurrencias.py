
def contar_ocurrencias(lista,valor):
    contador = 0
    for elemento in lista:
        if elemento in lista:
            if elemento == valor:
                contador += 1
    return contador

print(contar_ocurrencias([1, 4, 2, 4, 4, 7], 4))
print(contar_ocurrencias(["a", "b", "a"], "c"))            
