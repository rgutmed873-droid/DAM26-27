
notas = [5, 8, 8, 3, 9, 5, 7]

print("Lista original: ", notas)

notas.sort(reverse=True)
print("Notas ordenadas: ", notas)

notas.insert(1,5)
print("Con recuperación: ", notas)

notas.pop()
print("Sin la última: ", notas)