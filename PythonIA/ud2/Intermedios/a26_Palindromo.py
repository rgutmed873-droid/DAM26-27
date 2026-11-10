
def palindromo(palabra):
    palabra = palabra.lower()
    inversa = ""
    for letra in palabra:
        inversa = letra + inversa
    return inversa == palabra

print(palindromo("ana"))
print(palindromo("Perro"))
print(palindromo("oso"))   
print(palindromo("Vaca"))