import sys

euros = int(input("Dime cuantos euros quieres convertir: "))

dolar = euros * 1.14
libra = euros * 0.86
yen = euros * 180.21

print(f"La cantidad de euros a dolares es: {dolar: .2f}, dolares")
print(f"La cantidad de euros a libras es: {libra: .2f}, libras")
print(f"La cantidad de euros a yenes es: {yen: .2f}, yenes")
