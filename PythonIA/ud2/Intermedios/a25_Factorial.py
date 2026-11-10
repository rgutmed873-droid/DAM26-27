
def factorial(n):
    resultado = 1
    for i in range(2, n +1):
        resultado *= i
    return resultado

print(factorial(0))
print(factorial(1))
print(factorial(5))
print(factorial(7))
    